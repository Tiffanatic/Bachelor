using System;
using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public class ContactService : IContactService
    {
        private IUnitofWork _unitofWork;
        public ContactService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IEnumerable<Contact> GetAll()
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

        public Contact FindById(int i)
        {
            return _unitofWork.ContactRepository.GetbyId(i);
        }

        public void Update(Contact contact)
        {
            try
            {
                _unitofWork.ContactRepository.Update(contact);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public void Insert(Contact contact)
        {
            try
            {
                _unitofWork.ContactRepository.Insert(contact);
                _unitofWork.Commit();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}