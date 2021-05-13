using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models.Address;
using RapidTime.Services;

using Xunit;

namespace RapidTime.Tests
{
    public class CountryServiceTests
    {
        private readonly List<CountryEntity> _dummyData = new List<CountryEntity>()
        {
            new() {Id = 1, CountryName = "Danmark", CountryCode = "DK"},
            new() {Id = 2, CountryName = "Sverige", CountryCode = "SE"}
        };

        private readonly Mock<IUnitofWork> _mockUnitOfWork;
        private readonly Mock<IRepository<CountryEntity>> _mockCountryRepository;
        private readonly CountryService _countryService;
        private readonly Mock<ILogger<CountryService>> _logger; 
        
        public CountryServiceTests()
        {
            
            _mockCountryRepository = new Mock<IRepository<CountryEntity>>();
            _logger = new Mock<ILogger<CountryService>>();
            _mockUnitOfWork = new Mock<IUnitofWork>();

            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);
            _countryService = new CountryService(_mockUnitOfWork.Object, _logger.Object);
        }
        
        [Fact]
        public void ServiceShouldGetAllCountries()
        {
            //Arrange
            _mockCountryRepository.Setup(cr => cr.GetAll())
                .Returns(_dummyData);
            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);

            //Act
            var countries = _countryService.GetAllCountries();

            //Assert
            using (new AssertionScope())
            {
                countries.Should().HaveCount(2);
                countries.Should().OnlyHaveUniqueItems(c => c.Id);
                countries.Should().NotContainNulls();
                countries.Should().Contain(c => c.Id == 1)
                    .And.Contain(c => c.Id == 2);
            }
        }

        [Fact]
        public void ServiceShouldDeleteCountry()
        {
            //Arrange
            _mockCountryRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);

            CountryEntity countryEntity = new CountryEntity() {Id = 1};
            CountryService countryService = new CountryService(_mockUnitOfWork.Object, _logger.Object);
            
            //act
            countryService.DeleteCountry(countryEntity.Id);
            
            //assert
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
            _mockCountryRepository.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData("Danmark")]
        [InlineData("SE")]
        [InlineData("Dan")]
        [InlineData("SVERIGE")]
        public void ServiceShouldGetCountryByNameOrCode(string input)
        {
            //Arrange
            _mockCountryRepository.Setup(cr => cr.GetAll())
                .Returns(_dummyData);
            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);
            
            //act
            CountryEntity[] countries = _countryService.GetCountryByNameOrCountryCode(input);
            
            //assert
            countries.Should().HaveCount(1);
            countries.Should().NotBeNull();
            
        }

        [Fact]
        public void ServiceShouldFindById()
        {
            _mockCountryRepository.Setup(cr => cr.GetAll())
                .Returns(_dummyData);
            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);
            
            //act
            CountryEntity country = _countryService.FindById(1);
            
            //assert
            country.Should().BeEquivalentTo(_dummyData[0]);
        }

        [Fact]
        public void ServiceShouldInsertCountry()
        {
            //Arrange
            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);
            CountryEntity countryEntity = new() {Id = 3, CountryName = "Norge", CountryCode = "NO"};
            
            //act
            _countryService.Insert(countryEntity);
            
            //Assert
            _mockCountryRepository.Verify(_ => _.Insert(countryEntity), Times.Once);
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldUpdateCountry()
        {
            //Arrange
            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);
            CountryEntity countryEntity = new() {Id = 1, CountryCode = "DK", CountryName = "Danmark"};
            
            //act
            _countryService.Update(countryEntity);
            
            //assert
            _mockCountryRepository.Verify(_ => _.Update(countryEntity), Times.Once);
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailOnUpdate()
        {
            //Arrange
            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);
            CountryEntity countryEntity = null;
            
            //act & assert
            //_countryService.Invoking(cs => cs.Insert(countryEntity.Id).Should().Throw<NullReferenceException>());
        }

        [Fact]
        public void ServiceShouldReturnAllCountries()
        {
            //Arrange
            _mockCountryRepository.Setup(cr => cr.GetAll())
                .Returns(_dummyData);
            _mockUnitOfWork.Setup(_ => _.CountryRepository).Returns(_mockCountryRepository.Object);
            
            //Act
            var response = _countryService.GetAllCountries();

            //Assert
            response.Should().BeEquivalentTo(_dummyData);
            response.Should().HaveCount(2);
            response.Should().NotContainNulls();
        }
    }
}