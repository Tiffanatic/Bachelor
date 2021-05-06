﻿using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface IContactService
    {
        IEnumerable<ContactEntity> GetAll();
        void Delete(int contactId);
        ContactEntity FindById(int i);
        void Update(ContactEntity contactEntity);
        void Insert(ContactEntity contactEntity);
    }
}