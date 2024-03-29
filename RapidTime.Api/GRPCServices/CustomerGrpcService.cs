﻿using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;
using Enum = System.Enum;

namespace RapidTime.Api.GRPCServices
{
    public class CustomerGrpcService : Customer.CustomerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ICompanyTypeService _companyTypeService;
        private readonly ILogger<CustomerGrpcService> _logger;
        private readonly ICustomerContactService _customerContactService;

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
                CvrNumber = request.CVRNummer,
                CompanyTypeId = request.CompanyType.Id,
                YearlyReview = request.YearlyReview.ToDateTime(),
                InvoiceMail = request.InvoiceEmail,
                InvoiceCurrency = Enum.Parse<CustomerEntity.InvoiceCurrencyEnum>(request.InvoiceCurrency) 
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
                    CompanyTypeName = _companyTypeService.FindById(customerEntity.CompanyTypeId).CompanyTypeName
                },
                YearlyReview = customerEntity.YearlyReview.ToUniversalTime().ToTimestamp(),
                InvoiceCurrency = customerEntity.InvoiceCurrency.ToString(),
                InvoiceEmail = customerEntity.InvoiceMail
            };
                
            //response.Response.Contact.AddRange(CustomerContactsToContactBaseRepeatedField(customerEntity.CustomerContacts));
            return response;
        }

        public override Task<CustomerContactResponse> AddContactCustomer(CustomerContactRequest request, ServerCallContext context)
        {
            var res = _customerContactService.CreateCustomerContact(request.CustomerId, request.ContactId);

            return Task.FromResult(new CustomerContactResponse() {Success = res});

        }

        public override Task<MultiCustomerResponse> GetAllCustomers(Empty request, ServerCallContext context)
        {
            var customerEntities = _customerService.GetAllCustomers();
            MultiCustomerResponse response = new();
        
            foreach (CustomerEntity entity in customerEntities)
            {
                response.Response.Add(new CustomerResponse()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    CompanyType = new()
                    {
                        Id = entity.CompanyTypeId,
                        CompanyTypeName = entity.CompanyTypeEntity.CompanyTypeName
                    },
                    YearlyReview = entity.YearlyReview.ToUniversalTime().ToTimestamp(),
                    InvoiceEmail = entity.InvoiceMail,
                    InvoiceCurrency = entity.InvoiceCurrency.ToString()
                });
            }
            
            return Task.FromResult(response);
        }

        public override Task<MultiContactsResponse> GetContactsForCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            var contacts = _customerContactService.GetContactsForCustomer(request.Id);
            MultiContactsResponse response = new MultiContactsResponse();

            foreach (CustomerContact contact in contacts)
            {
                response.Contacts.Add(new ContactResponse()
                {
                    Email = contact.ContactEntity.Email,
                    FirstName = contact.ContactEntity.Firstname,
                    LastName = contact.ContactEntity.Lastname,
                    Id = contact.ContactEntity.Id,
                    TelephoneNumber = contact.ContactEntity.TelephoneNumber
                });
            }
            
            return Task.FromResult(response);
        }
        //TODO: Implement DeleteContactForCustomer
    }
}