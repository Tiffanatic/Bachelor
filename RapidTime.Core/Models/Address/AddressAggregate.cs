namespace RapidTime.Core.Models.Address
{
    public class AddressAggregate : BaseEntity
    {
        public City City { get; set; }
        public Country Country { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        
    }
}