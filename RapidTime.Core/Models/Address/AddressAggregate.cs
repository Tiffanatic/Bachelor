namespace RapidTime.Core.Models.Address
{
    public class AddressAggregate
    {
        public int Id { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }
        public string Street { get; set; }
        public int StreetNumber { get; set; }
        
    }
}