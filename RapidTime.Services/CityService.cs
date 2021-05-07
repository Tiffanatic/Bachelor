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

        public IEnumerable<CityEntity> GetAllCities()
        {
            return _unitofWork.CityRepository.GetAll();
        }

        public void DeleteCity(int id)
        {
            _unitofWork.CityRepository.Delete(id);
            _unitofWork.Commit();
        }

        public CityEntity[] FindCityByNameOrPostalCode(string NameOrPostalCode)
        {
            List<CityEntity> cities = GetAllCities().ToList();
            int postalCode;
            CityEntity[] city;
            if (int.TryParse(NameOrPostalCode, out postalCode))
            {
                city = cities.FindAll(x => x.PostalCode == postalCode.ToString()).ToArray();
                return city;
            }

            city = cities.FindAll(x => true).Where(x => x.CityName.Contains(NameOrPostalCode)).ToArray();
            return city;
        }

        public CityEntity FindById(int id)
        {
            return _unitofWork.CityRepository.GetbyId(id);
        }

        public int Insert(CityEntity cityEntity)
        {
            
            var id = _unitofWork.CityRepository.Insert(cityEntity);
            _unitofWork.Commit();
            return id;
        }

        public void Update(CityEntity cityEntity)
        {
            _unitofWork.CityRepository.Update(cityEntity);
            _unitofWork.Commit();
        }
    }
}