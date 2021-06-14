using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<CustomerEntity> GetAll();
        CustomerEntity Insert(CustomerEntity customerEntity);
        CustomerEntity GetbyId(int id);
        void Update(CustomerEntity customerEntity);
        void Delete(int id);
    }
}