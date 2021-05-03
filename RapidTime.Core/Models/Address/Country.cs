using System.Collections.Generic;

namespace RapidTime.Core.Models.Address
{
    public class Country : BaseEntity
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        
        public IList<City> Cities { get; set; }
        
        public IList<AddressAggregate> AddressAggregates { get; set; }
    }
}