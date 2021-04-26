using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Services
{
    public interface IAddressAggregateService
    {
        IEnumerable<AddressAggregate> GetAll();
        void Delete(int id);
    }
}