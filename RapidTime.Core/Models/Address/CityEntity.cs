using System.Collections.Generic;

namespace RapidTime.Core.Models.Address
{
    public class CityEntity : BaseEntity
    {
        public string PostalCode { get; set; }
        public string CityName { get; set; }
        
        public int CountryId { get; set; }
        public CountryEntity CountryEntity { get; set; }
        
        public IList<AddressAggregateEntity> AddressAggregates { get; set; }
    }
}