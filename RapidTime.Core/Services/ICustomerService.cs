using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core.Services
{
    public interface ICustomerService
    {
        Customer GetById(int i);
        IEnumerable<Customer> GetAllCustomers();
        void Delete(int customerId);
        void Insert(Customer customer);
        void Update(Customer customer);
    }
}