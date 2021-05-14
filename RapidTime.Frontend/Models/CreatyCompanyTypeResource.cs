using System.ComponentModel.DataAnnotations;

namespace RapidTime.Frontend.Models
{
    public class CreatyCompanyTypeResource
    {
        [Required(ErrorMessage = "Firmatype navn er påkrævet")]
        public string Name { get; set; }
    }
}