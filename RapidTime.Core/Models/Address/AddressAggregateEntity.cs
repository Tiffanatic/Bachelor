namespace RapidTime.Core.Models.Address
{
    public class AddressAggregateEntity : BaseEntity
    {
        public string Street { get; set; }
        public int CityId { get; set; }
        public CityEntity CityEntity { get; set; }
        public int CountryId { get; set; }
        public CountryEntity CountryEntity { get; set; }
        public int CustomerId { get; set; }
        public CustomerEntity CustomerEntity { get; set; }
        
    }
}