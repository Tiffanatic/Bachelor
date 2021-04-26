using System.Collections.Generic;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Models.Assignment;
using RapidTime.Core.Models.AssignmentType;
using RapidTime.Core.Models.Customer;

namespace RapidTime.Tests
{
    public class TimeRegistrationServiceTests
    {
        private Mock<IUnitofWork> _mockUnitOfWork;
        private Mock<IRepository<Assignment>> _mockAssignmentRepository;
        private Mock<IRepository<AssignmentType>> _mockAssignmentTypeRepository;
        private Mock<IRepository<AddressAggregate>> _mockAddressAggregateRepository;
        private Mock<IRepository<Country>> _mockCountryRepository;
        private Mock<IRepository<City>> _mockCityRepository;

        public TimeRegistrationServiceTests(Mock<IUnitofWork> mockUnitOfWork, Mock<IRepository<Assignment>> mockAssignmentRepository, Mock<IRepository<AssignmentType>> mockAssignmentTypeRepository, Mock<IRepository<AddressAggregate>> mockAddressAggregateRepository, Mock<IRepository<Country>> mockCountryRepository, Mock<IRepository<City>> mockCityRepository)
        {
            _mockUnitOfWork = mockUnitOfWork;
            _mockAssignmentRepository = mockAssignmentRepository;
            _mockAssignmentTypeRepository = mockAssignmentTypeRepository;
            _mockAddressAggregateRepository = mockAddressAggregateRepository;
            _mockCountryRepository = mockCountryRepository;
            _mockCityRepository = mockCityRepository;
        }

        public async void ServiceShouldReturnCorrectTime()
        {
            //Arrange
            List<AssignmentType> assignments = new List<AssignmentType>()
            {
                new AssignmentType() {Name = "Revision", Id = 0, Number = "100", InvoiceAble = true},
                new AssignmentType() {Name = "Assistance", Id = 1, Number = "200", InvoiceAble = true},
                new AssignmentType() {Name = "kontor", Id = 2, Number = "300", InvoiceAble = false}
            };

            List<City> citiesList = new List<City>()
            {
                new City() {Id = 0, CityName = "Kongens Lyngby", PostalCode = "2800"},
                new City() {Id = 1, CityName = "Seattle", PostalCode = "WA 98052"},
                new City() {Id = 2, CityName = "København K", PostalCode = "1165"},
                new City() {Id = 3, CityName = "Mountain View", PostalCode = "CA 94043"},
                new City() {Id = 4, CityName = "Stockholm", PostalCode = "111 22"}
            };

            List<Country> countryList = new List<Country>()
            {
                new Country() {Id = 0, CountryCode = "DK", CountryName = "Denmark"},
                new Country() {Id = 1, CountryCode = "US", CountryName = "United States of America"},
                new Country() {Id = 2, CountryCode = "SE", CountryName = "Sweden"}
            };
            

            List<AddressAggregate> addressList = new List<AddressAggregate>()
            {
                new AddressAggregate()
                {
                    Id = 0,
                    Street = "Kanalvej",
                    StreetNumber = 7,
                    City = citiesList.Find( c => c.PostalCode == "2800"),
                    Country = countryList.Find(c => c.CountryCode == "DK")
                },
                new AddressAggregate()
                {
                    Id = 1,
                    Street = "Sankt Petri Passage",
                    StreetNumber = 5,
                    City = citiesList.Find(c => c.PostalCode == "1165"),
                    Country = countryList.Find(c => c.CountryCode == "DK")
                },
                new AddressAggregate()
                {
                    Id = 2,
                    Street = "Kungsbroen",
                    StreetNumber = 2,
                    City = citiesList.Find(c => c.PostalCode == "111 22"),
                    Country = countryList.Find(c => c.CountryCode == "SE")
                },
                new AddressAggregate()
                {
                    Id = 3,
                    Street = "Amphitheatre Parkway",
                    StreetNumber = 1600,
                    City = citiesList.Find(c => c.PostalCode == "CA 94043"),
                    Country = countryList.Find(c => c.CountryCode == "US")
                },
                new AddressAggregate()
                {
                    Id = 4,
                    Street = "Microsoft Way",
                    StreetNumber = 1,
                    City = citiesList.Find(c => c.PostalCode == "WA 98052"),
                    Country = countryList.Find(c => c.CountryCode == "US")
                }
            };
            
            Customer google = new() {Name = "Google"};
            Customer microsoft = new() {Name = "Microsoft"};
            

            //Act

            //Assert
        }
    }
}