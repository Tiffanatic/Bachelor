using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;

namespace RapidTime.Data
{
    public class UnitofWork : IUnitofWork
    {
        private readonly RapidTimeDbContext _context;
        private IRepository<Price> _priceRepository;
        private IRepository<Contact> _contactRepository;
        private IRepository<Customer> _customerRepository;
        private IRepository<CompanyType> _companyTypeRepository;
        private IRepository<AssignmentType> _assignmentTypeRepository;
        private IRepository<Assignment> _assignmentRepository;
        private IRepository<Country> _countryRepository;
        private IRepository<City> _cityRepository;
        private IRepository<AddressAggregate> _addressAggregateRepository;
        
        public UnitofWork(RapidTimeDbContext context)
        {
            _context = context;
        }

        public IRepository<AddressAggregate> AddressAggregateRepository
        {
            get { return _addressAggregateRepository ??= new Repository<AddressAggregate>(_context); }
        }
        public IRepository<City> CityRepository
        {
            get { return _cityRepository ??= new Repository<City>(_context); }
        }
        public IRepository<Country> CountryRepository
        {
            get { return _countryRepository ??= new Repository<Country>(_context); }
        }
        public IRepository<Assignment> AssignmentRepository
        {
            get
            { return _assignmentRepository ??= new Repository<Assignment>(_context); }
        }

        public IRepository<AssignmentType> AssignmentTypeRepository
        {
            get { return _assignmentTypeRepository ??= new Repository<AssignmentType>(_context); }
        }
        
        public IRepository<CompanyType> CompanyTypeRepository
        {
            get { return _companyTypeRepository ??= new Repository<CompanyType>(_context); }
        }

        public IRepository<Contact> ContactRepository
        {
            get { return _contactRepository ??= new Repository<Contact>(_context); }
        }

        public IRepository<Customer> CustomerRepository
        {
            get { return _customerRepository ??= new Repository<Customer>(_context); }
        }

        public IRepository<Price> PriceRepository
        {
            get { return _priceRepository ??= new Repository<Price>(_context); }
        }

        public void Commit()
        {
            _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            _context.Dispose();
        }
    }
}