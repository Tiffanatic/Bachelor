using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core
{
    public interface IUnitofWork
    {
        IRepository<Contact> ContactRepository { get; }
        IRepository<Price> PriceRepository { get; }
        IRepository<Customer> CustomerRepository { get; }
        IRepository<CompanyType> CompanyTypeRepository { get; }
        IRepository<AssignmentType> AssignmentTypeRepository { get; }
        IRepository<Assignment> AssignmentRepository { get; }
        IRepository<Country> CountryRepository { get; }
        IRepository<City> CityRepository { get; }
        
        void Commit();
        void Rollback();
    }
}