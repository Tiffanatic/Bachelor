namespace RapidTime.Core.Models
{
    public class CustomerContact : BaseEntity
    {
        public int CustomerId { get; set; }
        public CustomerEntity CustomerEntity { get; set; }
        
        public int ContactId { get; set; }
        public ContactEntity ContactEntity { get; set; }
    }
}