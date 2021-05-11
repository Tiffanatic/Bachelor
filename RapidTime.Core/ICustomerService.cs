using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface ICustomerService
    {
        CustomerEntity GetById(int i);
        IEnumerable<CustomerEntity> GetAllCustomers();
        void Delete(int customerId);
        int Insert(CustomerEntity customerEntity);
        void Update(CustomerEntity customerEntity);
    }
}