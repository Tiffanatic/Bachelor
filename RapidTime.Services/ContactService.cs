using System;
using System.Collections.Generic;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Services
{
    public class ContactService : IContactService
    {
        private IUnitofWork _unitofWork;
        private ICustomerService _customerService;
        public ContactService(IUnitofWork unitofWork, ICustomerService customerService)
        {
            _unitofWork = unitofWork;
            _customerService = customerService;
        }

        public IEnumerable<ContactEntity> GetAll()
        {
            return _unitofWork.ContactRepository.GetAll();
        }

        public void Delete(int contactId)
        {
            if (contactId == null) throw new ArgumentNullException(nameof(contactId));
            try
            {
                _unitofWork.ContactRepository.Delete(contactId);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message, e.StackTrace);
            }
        }

        public ContactEntity FindById(int i)
        {
            return _unitofWork.ContactRepository.GetbyId(i);
        }

        public void Update(ContactEntity contactEntity)
        {
            try
            {
                _unitofWork.ContactRepository.Update(contactEntity);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public int Insert(ContactEntity contactEntity)
        {
            try
            {
                
                var id =_unitofWork.ContactRepository.Insert(contactEntity);
                _unitofWork.Commit();
                return id.Id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public CustomerContact AddContactToCustomer(ContactEntity contactEntity, int customerId)
        {
            try
            {

                var customer = _customerService.GetById(customerId);
                var customerContact = new CustomerContact()
                {
                    ContactId = contactEntity.Id,
                    CustomerId = customer.Id
                };
                var id = _unitofWork.CustomerContactRepository.Insert(customerContact);
                _unitofWork.Commit();
                return _unitofWork.CustomerContactRepository.GetbyId(id.Id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        } 
    }
}