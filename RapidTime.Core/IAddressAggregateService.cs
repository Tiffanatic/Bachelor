using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Services
{
    public interface IAddressAggregateService
    {
        IEnumerable<AddressAggregateEntity> GetAll();
        void Delete(int id);
        AddressAggregateEntity FindById(int id);
        void Update(AddressAggregateEntity addressAggregateEntity);
        void Insert(AddressAggregateEntity addressAggregateEntity);
    }
}