using Grpc.Net.Client;
using RapidTime.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace RapidTime.Frontend.ProtoClients
{
    public class CustomerGrpcClient
    {
        public Task<CustomerResponse> CreateCustomer(CreateCustomerResource createCustomerResource)
        {
            var client = GetClient();
            CompanyType.CompanyTypeClient companyTypeClient = new CompanyType.CompanyTypeClient(GrpcChannel.ForAddress("https://localhost:5001"));
        
            var companyType = companyTypeClient.GetCompanyType(new GetCompanyTypeRequest()
            {
                Id = createCustomerResource.CompanyTypeId
            });
            CompanyTypeResponse responseMapped = new()
            {
                Id = companyType.Id,
                CompanyTypeName = companyType.CompanyTypeName
            };


            var res = client.CreateCustomer(new CreateCustomerRequest()
            {
                Name = createCustomerResource.Name,
                CVRNummer = createCustomerResource.CvrNumber,
                CompanyType = responseMapped,
                YearlyReview = createCustomerResource.YearlyReview.ToTimestamp(),
                InvoiceEmail = createCustomerResource.InvoiceEmail
            });
    
            return Task.FromResult(res);
        }

        private Customer.CustomerClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Customer.CustomerClient(channel);
            return client;
        }

        public List<CustomerResponse> GetAllCustomers()
        {
            var client = GetClient();
            // var response = client.GetAllCustomers(new Empty()).Response.ToList();
            // return response;

            var customers = client.GetAllCustomers(new Empty());
            List<CustomerResponse> customerResponses = new List<CustomerResponse>();
            foreach (var customer in customers.Response)
            {
                customerResponses.Add(customer);
            }

            return customerResponses;
        }

        public CustomerResponse GetCustomer(int id)
        {
            var client = GetClient();
            return client.GetCustomer(new GetCustomerRequest()
            {
                Id = id
            });
        }

        public void UpdateCustomer(UpdateCustomerRequest request)
        {
            var client = GetClient();
            client.UpdateCustomer(request);
        }

        public void DeleteCustomer(int customerId)
        {
            var client = GetClient();
            client.DeleteCustomer(new DeleteCustomerRequest(){Id = customerId});
        }

        public List<ContactResponse> GetContactsForCustomer(int customerId)
        {
            var client = GetClient();
            var contacts = client.GetContactsForCustomer(new GetCustomerRequest() {Id = customerId});

            List<ContactResponse> contactResponses = new List<ContactResponse>();

            foreach (ContactResponse contact in contacts.Contacts)
            {
                contactResponses.Add(new ContactResponse()
                {
                    Id = contact.Id,
                    Email = contact.Email,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    TelephoneNumber = contact.TelephoneNumber
                });
            }

            return contactResponses;
        }
        
        
        
        //TODO:
        // -Add Contact To Customer
        // -Delete Contact From Customer
    }
}
