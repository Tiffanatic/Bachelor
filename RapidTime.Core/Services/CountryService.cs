using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Services
{
    public class CountryService
    {
        private readonly IUnitofWork _unitofWork;

        public CountryService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _unitofWork.CountryRepository.getAll();
        }

        public void DeleteCountry(int countryId)
        {
            _unitofWork.CountryRepository.Delete(countryId);
            _unitofWork.Commit();
        }

        public Country[] GetCountryByNameOrCountryCode(string input)
        {
            var countries = GetAllCountries().ToList();
            if (input.Length < 3)
            {
                return countries.Where(x => x.CountryCode.Contains(input)).ToArray();
            }
            return countries.Where((x => x.CountryName.Contains(input))).ToArray();
        }

        public Country FindById(int id)
        {
            var countries = GetAllCountries().ToList();
            return countries.SingleOrDefault(c => c.Id == id);
        }

        public void Insert(Country country)
        {
            _unitofWork.CountryRepository.Insert(country);
            _unitofWork.Commit();
        }

        public void Update(Country country)
        {
            _unitofWork.CountryRepository.Update(country);
            _unitofWork.Commit();
        }
    }
}