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
        private ILogger<CustomerGrpcService> _logger;

        public CustomerGrpcService(ILogger<CustomerGrpcService> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
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
            _logger.LogInformation("Update Customer called on Id: {Id}", request.Request.Id);
            var customerToUpdate = _customerService.GetById(request.Request.Id);
            if (!String.IsNullOrEmpty(request.Request.Name)) 
                customerToUpdate.Name = request.Request.Name;
            if(!String.IsNullOrEmpty(request.Request.InvoiceEmail))
                customerToUpdate.InvoiceMail = request.Request.InvoiceEmail;
            if (request.Request.CompanyType.Id > 0)
                customerToUpdate.CompanyTypeId = request.Request.CompanyType.Id;
            if (!String.IsNullOrEmpty(request.Request.YearlyReview.ToString()))
                customerToUpdate.YearlyReview = request.Request.YearlyReview.ToDateTime();
            
            _customerService.Update(customerToUpdate);
            return Task.FromResult(new Empty());
        }

        private CustomerResponse CustomerEntityToCustomerResponse(CustomerEntity customerEntity)
        {
            CustomerResponse response = new()
            {
                Response =
                {
                    Id = customerEntity.Id,
                    Name = customerEntity.Name,
                    CompanyType =
                    {
                        Id = customerEntity.CompanyTypeId,
                        CompanyTypeName = customerEntity.CompanyTypeEntity.CompanyTypeName
                    },
                    YearlyReview = customerEntity.YearlyReview.ToTimestamp(),
                    InvoiceCurrency = (InvoiceCurrencyEnum) customerEntity.InvoiceCurrency,
                    InvoiceEmail = customerEntity.InvoiceMail,
                }
            };
            response.Response.Contact.AddRange(CustomerContactsToContactBaseRepeatedField(customerEntity.CustomerContacts));
            return response;
        }

        private RepeatedField<ContactBase> CustomerContactsToContactBaseRepeatedField(IList<CustomerContact> contacts)
        {
            RepeatedField<ContactBase> contactBases = new RepeatedField<ContactBase>();
            foreach (var contact in contacts)
            {
                contactBases.Add(new ContactBase()
                {
                    Email = contact.ContactEntity.Email,
                    FirstName = contact.ContactEntity.Firstname,
                    LastName = contact.ContactEntity.Lastname,
                    TelephoneNumber = contact.ContactEntity.TelephoneNumber
                });
            };
            return contactBases;
        }
        
    }
}