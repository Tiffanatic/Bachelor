using System.Collections.Generic;

namespace RapidTime.Core.Models.Address
{
    public class CountryEntity : BaseEntity
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        
        public IList<CityEntity> Cities { get; set; }
        
        public IList<AddressAggregateEntity> AddressAggregates { get; set; }
    }
}