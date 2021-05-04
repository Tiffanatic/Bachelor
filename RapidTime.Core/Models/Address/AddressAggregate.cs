namespace RapidTime.Core.Models.Address
{
    public class AddressAggregate : BaseEntity
    {
        public string Street { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        
    }
}