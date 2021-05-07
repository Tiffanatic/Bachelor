﻿using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface IRepository<T> where T : BaseEntity
    {

        
        IEnumerable<T> GetAll();
        T GetbyId(int id);
        int Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}