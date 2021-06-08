namespace RapidTime.Core
{
    public interface ICustomerContactService
    {
        bool CreateCustomerContact(int CustomerId, int ContactId);
        bool RemoveCustomerContact(int customerContactId);
    }
}