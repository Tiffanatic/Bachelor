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
            Customer.CustomerClient client = GetClient();
            CompanyType.CompanyTypeClient companyTypeClient = new CompanyType.CompanyTypeClient(GrpcChannel.ForAddress("https://localhost:5001"));
        
            var companyType = companyTypeClient.GetCompanyType(new GetCompanyTypeRequest()
            {
                Id = createCustomerResource.CompanyTypeId
            });
            CompanyTypeBase baseMapped = new()
            {
                Id = companyType.Response.Id,
                CompanyTypeName = companyType.Response.CompanyTypeName
            };
        
            var res = client.CreateCustomer(new CreateCustomerRequest()
            {
                Name = createCustomerResource.Name,
                CVRNummer = createCustomerResource.CVRNumber,
                CompanyType = baseMapped,
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
            return client.GetAllCustomers(new Empty()).Response.ToList();
        }

        public CustomerResponse GetCustomer(int id)
        {
            var client = GetClient();
            return client.GetCustomer(new GetCustomerRequest()
            {
                Id = id
            });
        }
        
        //TODO:
        // -Update Customer
        // -Delete Customer
        // -Get Contacts For Customer
        // -Add Contact To Customer
        // -Delete Contact From Customer
        // -
    }
}
