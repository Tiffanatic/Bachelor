

using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class AddressAggregateTests
    {
        public List<AddressAggregate> DummyData = new()
        {
            new AddressAggregate()
            {
                Id= 1, 
                City = new City() {Id = 1, PostalCode = "7100", CityName = "Vejle"}, 
                Country = new Country() {Id = 1, CountryCode = "DK", CountryName = "Danmark"},
                Street = "Skovgade",
                StreetNumber = "21 2. 2"
            },
            new AddressAggregate()
            {
                Id= 2, 
                City = new City() {Id = 1, PostalCode = "7100", CityName = "Vejle"}, 
                Country = new Country() {Id = 1, CountryCode = "DK", CountryName = "Danmark"},
                Street = "Thulevej",
                StreetNumber = "13"
            }
        };

        private Mock<IUnitofWork> _mockUnitOfWork;
        private Mock<IRepository<AddressAggregate>> _mockAddressAggregateRepository;
        private AddressAggregateService _addressAggregateService;
        
        public AddressAggregateTests()
        {
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _mockAddressAggregateRepository = new Mock<IRepository<AddressAggregate>>();
            _mockUnitOfWork.Setup(_ => _.AddressAggregateRepository).Returns(
                _mockAddressAggregateRepository.Object);
            _addressAggregateService = new AddressAggregateService(_mockUnitOfWork.Object);
        }
        [Fact]
        public void ServiceShouldReturnAllAddresses()
        {
            //Arrange
            _mockAddressAggregateRepository.Setup(cr => cr.getAll())
                .Returns(DummyData);

            //Act
            var address = _addressAggregateService.GetAll();
            
            //Assert
            using (new AssertionScope())
            {
                address.Should().HaveCount(2);
                address.Should().OnlyHaveUniqueItems(c => c.Id);
                address.Should().NotContainNulls();
                address.Should().Contain(c => c.Id == 1)
                    .And.Contain(c => c.Id == 2);
            }
        }

        [Fact]
        public void ServiceShouldDeleteAddress()
        {
            //Arrange
            _mockAddressAggregateRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            AddressAggregate aggregate = new() {Id = 1};
            //Act
            _addressAggregateService.Delete(aggregate.Id);
            //Assert
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
            _mockAddressAggregateRepository.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailOnDeleteAddress()
        {
            //Arrange
            _mockAddressAggregateRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            AddressAggregate aggregate = null;
            
            _addressAggregateService.Invoking(a =>
                a.Delete(aggregate.Id)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceShouldGetById()
        {
            _mockAddressAggregateRepository.Setup(cr
                => cr.GetbyId(It.IsAny<int>())).Returns(DummyData[0]);

            var addressAggregate = _addressAggregateService.FindById(1);

            using (new AssertionScope())
            {
                addressAggregate.Should().BeEquivalentTo(DummyData[0]);
            }
        }

        [Fact]
        public void ServiceShouldUpdateAddressAggregate()
        {
            _mockAddressAggregateRepository.Setup(cr
                => cr.Update(It.IsAny<AddressAggregate>()));

            //_addressAggregateService.Update();
        }
        
    }
}