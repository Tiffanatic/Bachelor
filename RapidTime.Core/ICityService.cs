using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Services
{
    public interface ICityService
    {
        IEnumerable<CityEntity> GetAllCities();
        CityEntity[] FindCityByNameOrPostalCode(string NameOrPostalCode);
        void DeleteCity(int id);
        int Insert(CityEntity cityEntity);
        CityEntity FindById(int id);
        void Update(CityEntity cityEntity);
    }
}