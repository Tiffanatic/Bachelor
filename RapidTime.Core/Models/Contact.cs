using System;

namespace RapidTime.Core.Models
{
    public class Contact : BaseEntity
    {
        
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }

        public string fullName()
        {
            return Firstname + " " + Lastname;
        }
    }
}