using System.ComponentModel.DataAnnotations;

namespace RapidTime.Frontend.Models
{
    public class CreateCountryResource
    {
        [Required(ErrorMessage = "Landekode er påkrævet")]
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "Navn er påkrævet")]
        public string CountryName { get; set; }
    }
}