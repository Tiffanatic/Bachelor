using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Services
{
    public class CustomerContactService : ICustomerContactService
    {
        private readonly ILogger<CustomerContactService> _logger;
        private readonly IUnitofWork _unitofWork;
        

        public CustomerContactService(ILogger<CustomerContactService> logger, IUnitofWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        public bool CreateCustomerContact(int CustomerId, int ContactId)
        {
            _logger.LogInformation("CreateCustomerContactCalled with parameters {ContactId}, {CustomerId}", ContactId, CustomerId);
            CustomerEntity customerEntity;
            ContactEntity contactEntity;

            try
            {
                customerEntity = _unitofWork.CustomerRepository.GetbyId(CustomerId);
                contactEntity = _unitofWork.ContactRepository.GetbyId(ContactId);
                
                _unitofWork.CustomerContactRepository.Insert(new CustomerContact()
                {
                    ContactId = contactEntity.Id,
                    CustomerId = customerEntity.Id
                });
                
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        public bool RemoveCustomerContact(int customerContactId)
        {
            _logger.LogInformation("RemoveCustomerContact called with id: {Id}", customerContactId);

            try
            {
                _unitofWork.CustomerContactRepository.Delete(customerContactId);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return true;
        }

        public List<CustomerContact> GetContactsForCustomer(int customerId)
        {
            _logger.LogInformation("GetContactsForCustomer called from CustomerContactService with id: {CustomerId}", customerId);
            
            try
            {
                List<CustomerContact> res = _unitofWork.CustomerContactRepository.GetAll().ToList().FindAll(x => x.CustomerId == customerId);
                return res;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}