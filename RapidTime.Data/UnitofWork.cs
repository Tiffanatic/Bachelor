using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Models.Auth;
using RapidTime.Core.Repositories;

namespace RapidTime.Data
{
    public class UnitofWork : IUnitofWork
    {
        private RapidTimeDbContext _context;
        private IRepository<PriceEntity> _priceRepository;
        private IRepository<ContactEntity> _contactRepository;
        private IRepository<CustomerEntity> _customerRepository;
        private IRepository<CompanyTypeEntity> _companyTypeRepository;
        private IRepository<AssignmentTypeEntity> _assignmentTypeRepository;
        private IAssignmentRepository _assignmentRepository;
        private IRepository<CountryEntity> _countryRepository;
        private IRepository<CityEntity> _cityRepository;
        private IRepository<AddressAggregateEntity> _addressAggregateRepository;
        private IRepository<TimeRecordEntity> _timeRecordRepository;
        private IRepository<CustomerContact> _customerContactRepository;

        public UnitofWork(RapidTimeDbContext context)
        {
            _context = context;
        }

        public IRepository<AddressAggregateEntity> AddressAggregateRepository
        {
            get { return _addressAggregateRepository ??= new Repository<AddressAggregateEntity>(_context); }
        }

        public IRepository<CustomerContact> CustomerContactRepository
        {
            get
            {
                return _customerContactRepository ??= new Repository<CustomerContact>(_context);
            }
        }

        public IRepository<TimeRecordEntity> TimeRecordRepository
        {
            get
            {
                return _timeRecordRepository ??= new Repository<TimeRecordEntity>(_context);
            }
        }

        public IRepository<CityEntity> CityRepository
        {
            get { return _cityRepository ??= new Repository<CityEntity>(_context); }
        }
        public IRepository<CountryEntity> CountryRepository
        {
            get { return _countryRepository ??= new Repository<CountryEntity>(_context); }
        }
        public IAssignmentRepository AssignmentRepository
        {
            get
            { return _assignmentRepository ??= new AssignmentRepository(_context); }
        }

        public IRepository<AssignmentTypeEntity> AssignmentTypeRepository
        {
            get { return _assignmentTypeRepository ??= new Repository<AssignmentTypeEntity>(_context); }
        }
        
        public IRepository<CompanyTypeEntity> CompanyTypeRepository
        {
            get { return _companyTypeRepository ??= new Repository<CompanyTypeEntity>(_context); }
        }

        public IRepository<ContactEntity> ContactRepository
        {
            get { return _contactRepository ??= new Repository<ContactEntity>(_context); }
        }

        public IRepository<CustomerEntity> CustomerRepository
        {
            get { return _customerRepository ??= new Repository<CustomerEntity>(_context); }
        }

        public IRepository<PriceEntity> PriceRepository
        {
            get { return _priceRepository ??= new Repository<PriceEntity>(_context); }
        }
        
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            _context.Dispose();
        }
        
        
    }
}