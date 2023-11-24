using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core
{
    public interface ICityService
    {
        IEnumerable<CityEntity> GetAllCities();
        CityEntity[] FindCityByNameOrPostalCode(string nameOrPostalCode);
        void DeleteCity(int id);
        int Insert(CityEntity cityEntity);
        CityEntity FindById(int id);
        void Update(CityEntity cityEntity);
    }
}