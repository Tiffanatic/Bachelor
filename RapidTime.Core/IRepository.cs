using System.Collections.Generic;
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