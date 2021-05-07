using System;
using System.Collections.Generic;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Services;

namespace RapidTime.Services
{
    public class ContactService : IContactService
    {
        private IUnitofWork _unitofWork;
        public ContactService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
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
                return id;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}