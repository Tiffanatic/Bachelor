using System.ComponentModel.DataAnnotations;

namespace RapidTime.Frontend.Models
{
    public class AuthenticationUserModel
    {
        [Required(ErrorMessage ="Email is Required")]
        public string Email { get; set; }
        
        public string Password { get; set; }

    }
}
