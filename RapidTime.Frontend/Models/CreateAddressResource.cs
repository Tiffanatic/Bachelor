using System.ComponentModel.DataAnnotations;

namespace RapidTime.Frontend.Models
{
    public class CreateAddressResource
    {
        [Required(ErrorMessage = "Addresse er påkrævet")] 
        public string Street { get; set; }
        [Required(ErrorMessage = "By er påkrævet")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "Land er påkrævet")]
        public string CountryName { get; set; }
    }
}