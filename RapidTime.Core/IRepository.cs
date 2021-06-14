using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface IRepository<T>
    {

        
        IEnumerable<T> GetAll();
        T GetbyId(int id);
        T Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}