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
        [Fact]
        public void ServiceShouldGetAllCountries()
        {
            //Arrange
            List<CountryEntity> DummyData = new List<CountryEntity>()
            {
                new() {Id = 1, CountryName = "Danmark", CountryCode = "DEN"},
                new() {Id = 2, CountryName = "Sverige", CountryCode = "SWE"}
            };

            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCountryRepository = new Mock<IRepository<CountryEntity>>();
            var mockLogger = new Mock<ILogger<CountryService>>();
            mockCountryRepository.Setup(cr => cr.GetAll())
                .Returns(DummyData);
            mockUnitofWork.Setup(_ => _.CountryRepository).Returns(mockCountryRepository.Object);

            CountryService countryService = new CountryService(mockUnitofWork.Object, mockLogger.Object);
            
            //Act

            var countries = countryService.GetAllCountries();

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
            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCountryRepository = new Mock<IRepository<CountryEntity>>();
            var mockLogger = new Mock<ILogger<CountryService>>();
            mockCountryRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            mockUnitofWork.Setup(_ => _.CountryRepository).Returns(mockCountryRepository.Object);

            CountryEntity countryEntity = new CountryEntity() {Id = 1};
            CountryService countryService = new CountryService(mockUnitofWork.Object, mockLogger.Object);
            //act
            countryService.DeleteCountry(countryEntity.Id);
            //assert
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
            mockCountryRepository.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData("Danmark")]
        [InlineData("SE")]
        [InlineData("Dan")]
        public void ServiceShouldGetCountryByNameOrCode(string input)
        {
            //Arrange
            List<CountryEntity> DummyData = new List<CountryEntity>()
            {
                new() {Id = 1, CountryName = "Danmark", CountryCode = "DK"},
                new() {Id = 2, CountryName = "Sverige", CountryCode = "SE"}
            };

            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCountryRepository = new Mock<IRepository<CountryEntity>>();
            var mockLogger = new Mock<ILogger<CountryService>>();
            mockCountryRepository.Setup(cr => cr.GetAll())
                .Returns(DummyData);
            mockUnitofWork.Setup(_ => _.CountryRepository).Returns(mockCountryRepository.Object);

            CountryService countryService = new CountryService(mockUnitofWork.Object, mockLogger.Object);
            
            //act
            CountryEntity[] countries = countryService.GetCountryByNameOrCountryCode(input);
            //assert
            countries.Should().HaveCount(1);
            countries.Should().NotBeNull();
            
        }

        [Fact]
        public void ServiceShouldFindById()
        {
            List<CountryEntity> DummyData = new List<CountryEntity>()
            {
                new() {Id = 1, CountryName = "Danmark", CountryCode = "DEN"},
                new() {Id = 2, CountryName = "Sverige", CountryCode = "SWE"}
            };

            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCountryRepository = new Mock<IRepository<CountryEntity>>();
            var mockLogger = new Mock<ILogger<CountryService>>();
            mockCountryRepository.Setup(cr => cr.GetAll())
                .Returns(DummyData);
            mockUnitofWork.Setup(_ => _.CountryRepository).Returns(mockCountryRepository.Object);

            CountryService countryService = new CountryService(mockUnitofWork.Object, mockLogger.Object);
            //act
            var country = countryService.FindById(1);
            
            //assert
            country.Should().BeEquivalentTo(DummyData[0]);
        }

        [Fact]
        public void ServiceShouldInsertCountry()
        {
            //Arrange
            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCountryRepository = new Mock<IRepository<CountryEntity>>();
            var mockLogger = new Mock<ILogger<CountryService>>();
            
            mockUnitofWork.Setup(_ => _.CountryRepository).Returns(mockCountryRepository.Object);

            CountryService countryService = new CountryService(mockUnitofWork.Object, mockLogger.Object);
            CountryEntity countryEntity = new() {Id = 3, CountryName = "Norge", CountryCode = "NO"};
            //act
            countryService.Insert(countryEntity);
            //Assert
            mockCountryRepository.Verify(_ => _.Insert(countryEntity), Times.Once);
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldUpdateCountry()
        {
            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCountryRepository = new Mock<IRepository<CountryEntity>>();
            var mockLogger = new Mock<ILogger<CountryService>>();
            mockUnitofWork.Setup(_ => _.CountryRepository).Returns(mockCountryRepository.Object);

            CountryService countryService = new CountryService(mockUnitofWork.Object, mockLogger.Object);
            CountryEntity countryEntity = new() {Id = 1, CountryCode = "DK", CountryName = "Danmark"};
            
            //act
            countryService.Update(countryEntity);
            
            //assert
            mockCountryRepository.Verify(_ => _.Update(countryEntity), Times.Once);
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailOnUpdate()
        {
            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCountryRepository = new Mock<IRepository<CountryEntity>>();
            var mockLogger = new Mock<ILogger<CountryService>>();
            mockUnitofWork.Setup(_ => _.CountryRepository).Returns(mockCountryRepository.Object);

            CountryService countryService = new CountryService(mockUnitofWork.Object, mockLogger.Object);
            CountryEntity countryEntity = null;
            //act
            countryService.Update(countryEntity);
            //assert
            
        }
    }
}