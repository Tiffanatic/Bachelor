using System.Collections.Generic;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Execution;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;
using RapidTime.Services;

namespace RapidTime.Tests
{
    public class CityServiceTests
    {
        [Fact]
        public void ServiceShouldGetAllCities()
        {
            //Arrange
            
            List<CityEntity> DummyData = new List<CityEntity>()
            {
                new() {PostalCode = "7100", CityName = "Vejle", Id = 1},
                new() {PostalCode = "8700", CityName = "Horsens", Id = 2},
                new() {PostalCode = "2670", CityName = "Greve", Id = 3}
            };

            var mockUnitofWork = new Mock<IUnitofWork>();
            var mockCityRepository = new Mock<IRepository<CityEntity>>();
            mockCityRepository.Setup(cr => cr.GetAll())
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
            
            var mockCityRepository = new Mock<IRepository<CityEntity>>();
            mockCityRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            CityEntity cityEntity = new CityEntity() {Id = 1};
            CityService cityService = new CityService(mockUnitofWork.Object);
            //act
            cityService.DeleteCity(cityEntity.Id);
            //assert
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
            mockCityRepository.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ServiceShouldGetTheCityByNameOrPostalCode()
        {
            List<CityEntity> DummyData = new List<CityEntity>()
            {
                new() {PostalCode = "7100", CityName = "Vejle", Id = 1},
                new() {PostalCode = "8700", CityName = "Horsens", Id = 2},
                new() {PostalCode = "2670", CityName = "Greve", Id = 3}
            };
            //arrange
            var mockCityRepository = new Mock<IRepository<CityEntity>>();
            mockCityRepository.Setup(cr => cr.GetAll()).Returns(DummyData);
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            CityEntity cityEntityToFind = new CityEntity() { PostalCode = "7100", CityName = "Vejle"};
            CityEntity newCityEntityToFind = new CityEntity() {PostalCode = "8700", CityName = "Horsens"};
            CityService cityService = new CityService(mockUnitofWork.Object);
            //act
            var foundCityByName = cityService.FindCityByNameOrPostalCode(cityEntityToFind.CityName);
            var foundCityByPostalCode = cityService.FindCityByNameOrPostalCode(cityEntityToFind.PostalCode);

            var newFoundCityByName = cityService.FindCityByNameOrPostalCode(newCityEntityToFind.CityName);
            var newFoundCityByPostalCode = cityService.FindCityByNameOrPostalCode(newCityEntityToFind.PostalCode);
            
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
            List<CityEntity> DummyData = new List<CityEntity>()
            {
                new() {PostalCode = "7100", CityName = "Vejle", Id = 1},
                new() {PostalCode = "8700", CityName = "Horsens", Id = 2},
                new() {PostalCode = "2670", CityName = "Greve", Id = 3}
            };
            //arrange
            var mockCityRepository = new Mock<IRepository<CityEntity>>();
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
            List<CityEntity> DummyData = new List<CityEntity>()
            {
                new() {PostalCode = "7100", CityName = "Vejle", Id = 1},
                new() {PostalCode = "8700", CityName = "Horsens", Id = 2},
                new() {PostalCode = "2670", CityName = "Greve", Id = 3}
            };
            //arrange
            var mockCityRepository = new Mock<IRepository<CityEntity>>();
            mockCityRepository.Setup(cr => cr.Update(It.IsAny<CityEntity>()));
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            
            CityService cityService = new CityService(mockUnitofWork.Object);
            CityEntity cityEntity = new CityEntity() {Id = 1, CityName = "Vejle Kommune", PostalCode = "7100"};
            
            //act
            cityService.Update(cityEntity);
            
            //assert
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
            mockCityRepository.Verify(_ => _.Update(cityEntity), Times.Once);
        }

        [Fact]
        public void ServiceShouldInsertCity()
        {
            //arrange 
            CityEntity cityEntity = new() {Id = 4};
            var mockCityRepository = new Mock<IRepository<CityEntity>>();
            mockCityRepository.Setup(cr => cr.Insert(It.IsAny<CityEntity>())).Returns(cityEntity);
            
            var mockUnitofWork = new Mock<IUnitofWork>();
            mockUnitofWork.Setup(_ => _.CityRepository).Returns(mockCityRepository.Object);
            
            CityService cityService = new CityService(mockUnitofWork.Object);
            //act
            cityService.Insert(cityEntity);
            //assert
            mockUnitofWork.Verify(_ => _.Commit(), Times.Once);
            mockCityRepository.Verify(_ => _.Insert(cityEntity), Times.Once);

        }
    }
}