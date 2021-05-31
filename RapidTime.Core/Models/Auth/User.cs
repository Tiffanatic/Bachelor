using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RapidTime.Core.Models.Auth
{
    public class User : IdentityUser<Guid>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public bool GdprDeleted { get; set; } = false;

        public DateTime DeleteDate { get; set; } = new DateTime(2099, 1, 1);
        
        public IList<PriceEntity> Prices { get; set; }
        
        public IList<AssignmentEntity> Assignments { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
            
        //Following fields are being inherited from IdentityUser
        /*
         * Email
         * Phone number
         */
    }
}