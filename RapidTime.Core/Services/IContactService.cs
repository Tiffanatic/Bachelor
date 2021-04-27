using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IContactService
    {
        IEnumerable<Contact> GetAll();
        void Delete(int contactId);
        Contact FindById(int i);
        void Update(Contact contact);
        void Insert(Contact contact);
    }
}