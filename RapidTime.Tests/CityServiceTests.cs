using System.Collections.Generic;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Execution;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;

namespace RapidTime.Tests
{
    public class CityServiceTests
    {
        [Fact]
        public void ServiceShouldGetAllCities()
        {
            //Arrange
            
            List<City> DummyData = new List<City>()
            {
                new() {PostalCode = "7100", CityName = "Vejle", Id = 1},
                new() {PostalCode = "8700", CityName = "Horsens", Id = 2},
                new() {PostalCode = "2670", CityName = "Greve", Id = 3}
            };

            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCityRepository = new Mock<IRepository<City>>();
            mockCityRepository.Setup(cr => cr.getAll())
                .Returns(DummyData);
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);

            CityService cityService = new CityService(mockUnitofWork.Object);

            //Act 

            var cities = cityService.GetAllCities();

            //Assert
            
            using (new AssertionScope())
            {
                cities.Should().HaveCount(3);
                cities.Should().OnlyHaveUniqueItems(c => c.Id);
                cities.Should().NotContainNulls();
                cities.Should().Contain(c => c.Id == 1)
                    .And.Contain(c => c.Id == 2)
                    .And.Contain(c => c.Id == 3);
            }
        }

        [Fact]
        public void ServiceShouldDeleteCity()
        {
            //arrange 
            
            var mockCityRepository = new Mock<IRepository<City>>();
            mockCityRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            City city = new City() {Id = 1};
            CityService cityService = new CityService(mockUnitofWork.Object);
            //act
            cityService.DeleteCity(city.Id);
            //assert
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
            mockCityRepository.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ServiceShouldGetTheCityByNameOrPostalCode()
        {
            List<City> DummyData = new List<City>()
            {
                new() {PostalCode = "7100", CityName = "Vejle", Id = 1},
                new() {PostalCode = "8700", CityName = "Horsens", Id = 2},
                new() {PostalCode = "2670", CityName = "Greve", Id = 3}
            };
            //arrange
            var mockCityRepository = new Mock<IRepository<City>>();
            mockCityRepository.Setup(cr => cr.getAll()).Returns(DummyData);
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            City cityToFind = new City() { PostalCode = "7100", CityName = "Vejle"};
            City newCityToFind = new City() {PostalCode = "8700", CityName = "Horsens"};
            CityService cityService = new CityService(mockUnitofWork.Object);
            //act
            var foundCityByName = cityService.FindCityByNameOrPostalCode(cityToFind.CityName);
            var foundCityByPostalCode = cityService.FindCityByNameOrPostalCode(cityToFind.PostalCode);

            var newFoundCityByName = cityService.FindCityByNameOrPostalCode(newCityToFind.CityName);
            var newFoundCityByPostalCode = cityService.FindCityByNameOrPostalCode(newCityToFind.PostalCode);
            
            //assert
            foundCityByName.Should().ContainSingle();
            foundCityByPostalCode.Should().ContainSingle();
            foundCityByName.Should().BeEquivalentTo(foundCityByPostalCode);
            
            newFoundCityByName.Should().ContainSingle();
            newFoundCityByPostalCode.Should().ContainSingle();
            newFoundCityByName.Should().BeEquivalentTo(newFoundCityByPostalCode);
            newFoundCityByName.Should().NotBeEquivalentTo(foundCityByName);
        }

        [Fact]
        public void ServiceShouldGetById()
        {
            List<City> DummyData = new List<City>()
            {
                new() {PostalCode = "7100", CityName = "Vejle", Id = 1},
                new() {PostalCode = "8700", CityName = "Horsens", Id = 2},
                new() {PostalCode = "2670", CityName = "Greve", Id = 3}
            };
            //arrange
            var mockCityRepository = new Mock<IRepository<City>>();
            mockCityRepository.Setup(cr => cr.GetbyId(It.IsAny<int>())).Returns(DummyData[0]);
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            
            CityService cityService = new CityService(mockUnitofWork.Object);
            //act
            var city = cityService.FindById(1);
            //assert
            city.Should().NotBeNull();
        }

        [Fact]
        public void ServiceShouldUpdateCity()
        {
            List<City> DummyData = new List<City>()
            {
                new() {PostalCode = "7100", CityName = "Vejle", Id = 1},
                new() {PostalCode = "8700", CityName = "Horsens", Id = 2},
                new() {PostalCode = "2670", CityName = "Greve", Id = 3}
            };
            //arrange
            var mockCityRepository = new Mock<IRepository<City>>();
            mockCityRepository.Setup(cr => cr.Update(It.IsAny<City>()));
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            
            CityService cityService = new CityService(mockUnitofWork.Object);
            City city = new City() {Id = 1, CityName = "Vejle Kommune", PostalCode = "7100"};
            
            //act
            cityService.Update(city);
            
            //assert
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
            mockCityRepository.Verify(_ => _.Update(city), Times.Once);
        }

        [Fact]
        public void ServiceShouldInsertCity()
        {
            //arrange 
            var mockCityRepository = new Mock<IRepository<City>>();
            mockCityRepository.Setup(cr => cr.Insert(It.IsAny<City>()));
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            
            CityService cityService = new CityService(mockUnitofWork.Object);
            City city = new() {Id = 4};
            //act
            cityService.Insert(city);
            //assert
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
            mockCityRepository.Verify(_ => _.Insert(city), Times.Once);

        }
    }
}