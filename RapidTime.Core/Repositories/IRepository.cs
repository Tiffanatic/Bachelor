using System.Collections.Generic;

namespace RapidTime.Core.Repositories
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