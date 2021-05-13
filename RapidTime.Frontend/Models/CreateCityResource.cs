using System.ComponentModel.DataAnnotations;

namespace RapidTime.Frontend.Models
{
    public class CreateCityResource
    {
        [Required(ErrorMessage = "Bynavn er påkrævet")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "Post nummer er påkrævet")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Land er påkrævet")]
        public string CountryName { get; set; }
    }
}