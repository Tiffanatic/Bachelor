using System.Collections.Generic;

namespace RapidTime.Core.Models.Address
{
    public class City : BaseEntity
    {
        public string PostalCode { get; set; }
        public string CityName { get; set; }
        
        public int CountryId { get; set; }
        public Country Country { get; set; }
        
        public IList<AddressAggregate> AddressAggregates { get; set; }
    }
}