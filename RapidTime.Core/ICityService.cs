using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Services
{
    public interface ICityService
    {
        IEnumerable<City> GetAllCities();
        City[] FindCityByNameOrPostalCode(string NameOrPostalCode);
        void DeleteCity(int id);
        void Insert(City city);
        City FindById(int id);
        void Update(City city);
    }
}