using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Api.GRPCServices
{
    public class CustomerGrpcService : Customer.CustomerBase
    {
        private ICustomerService _customerService;
        private ICompanyTypeService _companyTypeService;
        private ILogger<CustomerGrpcService> _logger;
        private ICustomerContactService _customerContactService;

        public CustomerGrpcService(ILogger<CustomerGrpcService> logger, ICustomerService customerService, ICompanyTypeService companyTypeService, ICustomerContactService customerContactService)
        {
            _logger = logger;
            _customerService = customerService;
            _companyTypeService = companyTypeService;

            _customerContactService = customerContactService;
        }

        public override Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Create Customer Called with the name: {Request}", request.Name);
            var customerToCreate = new CustomerEntity()
            {
                Name = request.Name,
                CVRNumber = request.CVRNummer,
                CompanyTypeId = request.CompanyType.Id,
                YearlyReview = request.YearlyReview.ToDateTime(),
                InvoiceMail = request.InvoiceEmail,
                InvoiceCurrency = (CustomerEntity.InvoiceCurrencyEnum) request.InvoiceCurrency
            };

            var id = _customerService.Insert(customerToCreate);
            var insertedCustomer = _customerService.GetById(id);
            

            return Task.FromResult(CustomerEntityToCustomerResponse(insertedCustomer));
            
        }

        public override Task<Empty> RequestDeletion(RequestDeleteCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Request For deletion Called with Customer Id {Id}", request.IdToDelete);
            
            _customerService.Delete(request.IdToDelete);

            return Task.FromResult(new Empty());
        }

        public override Task<CustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            var customer = _customerService.GetById(request.Id);

            return Task.FromResult(CustomerEntityToCustomerResponse(customer));
            
        }

        public override Task<Empty> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Delete Customer Called with id: {Id}", request.Id);
            _customerService.Delete(request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Update Customer called on Id: {Id}", request.Id);
            var customerToUpdate = _customerService.GetById(request.Id);
            if (!String.IsNullOrEmpty(request.Name)) 
                customerToUpdate.Name = request.Name;
            if(!String.IsNullOrEmpty(request.InvoiceEmail))
                customerToUpdate.InvoiceMail = request.InvoiceEmail;
            if (request.CompanyType.Id > 0)
                customerToUpdate.CompanyTypeId = request.CompanyType.Id;
            if (!String.IsNullOrEmpty(request.YearlyReview.ToString()))
                customerToUpdate.YearlyReview = request.YearlyReview.ToDateTime();
            
            _customerService.Update(customerToUpdate);
            return Task.FromResult(new Empty());
        }

        private CustomerResponse CustomerEntityToCustomerResponse(CustomerEntity customerEntity)
        {
            CustomerResponse response = new()
            {
                Id = customerEntity.Id,
                Name = customerEntity.Name,
                CompanyType = new()
                {
                    Id = customerEntity.CompanyTypeId,
                    CompanyTypeName = _companyTypeService.findById(customerEntity.CompanyTypeId).CompanyTypeName
                },
                YearlyReview = customerEntity.YearlyReview.ToUniversalTime().ToTimestamp(),
                InvoiceCurrency = (InvoiceCurrencyEnum) customerEntity.InvoiceCurrency,
                InvoiceEmail = customerEntity.InvoiceMail
            };
                
            //response.Response.Contact.AddRange(CustomerContactsToContactBaseRepeatedField(customerEntity.CustomerContacts));
            return response;
        }

        private RepeatedField<ContactResponse> CustomerContactsToContactBaseRepeatedField(IList<CustomerContact> contacts)
        {
            RepeatedField<ContactResponse> contactBases = new RepeatedField<ContactResponse>();
            foreach (var contact in contacts)
            {
                contactBases.Add(new ContactResponse()
                {
                    Email = contact.ContactEntity.Email,
                    FirstName = contact.ContactEntity.Firstname,
                    LastName = contact.ContactEntity.Lastname,
                    TelephoneNumber = contact.ContactEntity.TelephoneNumber
                });
            };
            return contactBases;
        }

        public override Task<CustomerContactResponse> AddContactCustomer(CustomerContactRequest request, ServerCallContext context)
        {
            var res = _customerContactService.CreateCustomerContact(request.CustomerId, request.ContactId);

            return Task.FromResult(new CustomerContactResponse() {Success = res});

        }

        // public override Task<MultiCustomerResponse> GetAllCustomers(Empty request, ServerCallContext context)
        // {
        //     var customerEntities = _customerService.GetAllCustomers();
        //     MultiCustomerResponse response = new MultiCustomerResponse();
        //
        //     foreach (CustomerEntity entity in customerEntities)
        //     {
        //         CompanyTypeEntity companyType = _companyTypeService.findById(entity.CompanyTypeId);
        //         CompanyTypeBase companyTypeBaseMapped = new CompanyTypeBase()
        //         {
        //             Id = companyType.Id,
        //             CompanyTypeName = companyType.CompanyTypeName
        //         };
        //
        //         var requestDeletionDate = _customerService.GetById(entity.Id);
        //         var requestDeletionDateMapped = requestDeletionDate.
        //         response.Response.Add(new CustomerResponse()
        //         {
        //             Response = new CustomerBase()
        //             {
        //                 Id = entity.Id,
        //                 Name = entity.Name,
        //                 CompanyType = companyTypeBaseMapped,
        //                 InvoiceCurrency = entity.InvoiceCurrency.,
        //                 InvoiceEmail = entity.InvoiceMail,
        //                 YearlyReview = entity.YearlyReview.ToTimestamp(),
        //                 RequestDeletionDate = entity.
        //             }
        //         });
        //     }
        // }

        public override Task<MultiContactsResponse> GetContactsForCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            return base.GetContactsForCustomer(request, context);
        }

        public override Task<Empty> DeleteContactCustomer(DeleteCustomerContactRequest request, ServerCallContext context)
        {
            return base.DeleteContactCustomer(request, context);
        }
    }
}