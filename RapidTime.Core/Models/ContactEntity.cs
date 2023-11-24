using System.Collections.Generic;

namespace RapidTime.Core.Models
{
    public class ContactEntity : BaseEntity
    {
        
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public IList<CustomerContact> CustomerContacts { get; set; }
        public string FullName()
        {
            return Firstname + " " + Lastname;
        }
    }
}