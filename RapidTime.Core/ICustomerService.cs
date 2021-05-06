using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface ICustomerService
    {
        CustomerEntity GetById(int i);
        IEnumerable<CustomerEntity> GetAllCustomers();
        void Delete(int customerId);
        void Insert(CustomerEntity customerEntity);
        void Update(CustomerEntity customerEntity);
    }
}