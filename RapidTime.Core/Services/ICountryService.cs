using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Services
{
    public interface ICountryService
    {
        IEnumerable<Country> GetAllCountries();
        void DeleteCountry(int countryId);
        Country[] GetCountryByNameOrCountryCode(string input);
        Country FindById(int id);
        void Insert(Country country);
        void Update(Country country);
        
    }
}