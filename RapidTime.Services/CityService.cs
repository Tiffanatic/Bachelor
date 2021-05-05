using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RapidTime.Core;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;

namespace RapidTime.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitofWork _unitofWork;

        public CityService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IEnumerable<City> GetAllCities()
        {
            return _unitofWork.CityRepository.GetAll();
        }

        public void DeleteCity(int id)
        {
            _unitofWork.CityRepository.Delete(id);
            _unitofWork.Commit();
        }

        public City[] FindCityByNameOrPostalCode(string NameOrPostalCode)
        {
            List<City> cities = GetAllCities().ToList();
            int postalCode;
            City[] city;
            if (int.TryParse(NameOrPostalCode, out postalCode))
            {
                city = cities.FindAll(x => x.PostalCode == postalCode.ToString()).ToArray();
                return city;
            }

            city = cities.FindAll(x => true).Where(x => x.CityName.Contains(NameOrPostalCode)).ToArray();
            return city;
        }

        public City FindById(int id)
        {
            return _unitofWork.CityRepository.GetbyId(id);
        }

        public void Insert(City city)
        {
            _unitofWork.CityRepository.Insert(city);
            _unitofWork.Commit();
        }

        public void Update(City city)
        {
            _unitofWork.CityRepository.Update(city);
            _unitofWork.Commit();
        }
    }
}