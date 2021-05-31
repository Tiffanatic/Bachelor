using System.ComponentModel.DataAnnotations;

namespace RapidTime.Frontend.Models
{
    public class CreateContactResource
    {
        [Required(ErrorMessage = "Fornavn er påkrævet")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Efternavn er påkrævet")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email er påkrævet")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefonnummer er påkrævet")]
        public string Telephonenumber { get; set; }

    }
}