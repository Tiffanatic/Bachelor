using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core
{
    public interface ICountryService
    {
        IEnumerable<CountryEntity> GetAllCountries();
        void DeleteCountry(int countryId);
        CountryEntity[] GetCountryByNameOrCountryCode(string input);
        CountryEntity FindById(int id);
        int Insert(CountryEntity countryEntity);
        void Update(CountryEntity countryEntity);
        
    }
}