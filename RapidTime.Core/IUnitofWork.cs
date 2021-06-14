using System;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Models.Auth;
using RapidTime.Core.Repositories;

namespace RapidTime.Core
{
    public interface IUnitofWork
    {
        IRepository<ContactEntity> ContactRepository { get; }
        IRepository<PriceEntity> PriceRepository { get; }
        IRepository<CustomerEntity> CustomerRepository { get; }
        IRepository<CompanyTypeEntity> CompanyTypeRepository { get; }
        IRepository<AssignmentTypeEntity> AssignmentTypeRepository { get; }
        //IRepository<AssignmentEntity> AssignmentRepository { get; }
        IAssignmentRepository AssignmentRepository { get; }
        IRepository<CountryEntity> CountryRepository { get; }
        IRepository<CityEntity> CityRepository { get; }
        IRepository<AddressAggregateEntity> AddressAggregateRepository { get; }
        IRepository<CustomerContact> CustomerContactRepository { get; }
        IRepository<TimeRecordEntity> TimeRecordRepository { get; }
        
        void Commit();
        void Rollback();
    }
}