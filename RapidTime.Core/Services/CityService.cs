using System.Collections;
using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Services
{
    public class CityService
    {
        private readonly IUnitofWork _unitofWork;

        public CityService(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public IEnumerable<City> GetAllCities()
        {
            return _unitofWork.CityRepository.getAll();
        }
    }
}