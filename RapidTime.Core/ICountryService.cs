using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Services
{
    public interface ICountryService
    {
        IEnumerable<CountryEntity> GetAllCountries();
        void DeleteCountry(int countryId);
        CountryEntity[] GetCountryByNameOrCountryCode(string input);
        CountryEntity FindById(int id);
        void Insert(CountryEntity countryEntity);
        void Update(CountryEntity countryEntity);
        
    }
}