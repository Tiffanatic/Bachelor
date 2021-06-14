using System.Collections.Generic;
using RapidTime.Core.Models;

namespace RapidTime.Core
{
    public interface ICustomerContactService
    {
        bool CreateCustomerContact(int CustomerId, int ContactId);
        bool RemoveCustomerContact(int customerContactId);
        List<CustomerContact> GetContactsForCustomer(int customerId);
    }
}