using System.Collections.Generic;
using Xunit;
using Moq;
using FluentAssertions;
using FluentAssertions.Execution;
using RapidTime.Core;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;

namespace RapidTime.Tests
{
    public class CityServiceTests
    {
        [Fact]
        public async void ServiceShouldGetAllCities()
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
    }
}