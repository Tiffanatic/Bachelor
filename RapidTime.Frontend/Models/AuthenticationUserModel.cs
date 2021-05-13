using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RapidTime.Frontend.Models
{
    public class AuthenticationUserModel
    {
        [Required(ErrorMessage ="Email is Required")]
        public string Email { get; set; }
        
        public string Password { get; set; }

    }
}
