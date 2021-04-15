using System;
using Microsoft.AspNetCore.Identity;

namespace RapidTime.Core.Models.Auth
{
    public class User : IdentityUser<Guid>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public bool GdprDeleted { get; set; }

        public DateTime? DeleteDate { get; set; } = null;
        
        
        
        //Following fields are being inherited from IdentityUser
        /*
         * Email
         * Phone number
         */

        public string fullName()
        {
            return Firstname + " " + Lastname;
        }

        public bool GDPRDelete()
        {
            try
            {
                Firstname = "Deleted User";
                Lastname = "";
                Email = "";
                PhoneNumber = "";
                GdprDeleted = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " \n" + ex.StackTrace);
            }

            return true;
        }

        public DateTime SetDeleteDate(DateTime date)
        {
            this.DeleteDate = date;
            return date;
        } 
    }
}