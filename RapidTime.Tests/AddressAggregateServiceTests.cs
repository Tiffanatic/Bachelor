using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Repositories;
using RapidTime.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class AddressAggregateTests
    {
        public List<AddressAggregateEntity> DummyData = new()
        {
            new AddressAggregateEntity()
            {
                Id= 1, 
                CityEntity = new CityEntity() {Id = 1, PostalCode = "7100", CityName = "Vejle"}, 
                CountryEntity = new CountryEntity() {Id = 1, CountryCode = "DK", CountryName = "Danmark"},
                Street = "Skovgade 21, 2, 2",
                
            },
            new AddressAggregateEntity()
            {
                Id= 2, 
                CityEntity = new CityEntity() {Id = 1, PostalCode = "7100", CityName = "Vejle"}, 
                CountryEntity = new CountryEntity() {Id = 1, CountryCode = "DK", CountryName = "Danmark"},
                Street = "Thulevej 13",
                
            }
        };

        private readonly Mock<IUnitofWork> _mockUnitOfWork;
        private readonly Mock<IRepository<AddressAggregateEntity>> _mockAddressAggregateRepository;
        private readonly AddressAggregateService _addressAggregateService;
        
        public AddressAggregateTests()
        {
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _mockAddressAggregateRepository = new Mock<IRepository<AddressAggregateEntity>>();
            
            _mockUnitOfWork.Setup(w => w.AddressAggregateRepository).Returns(
                _mockAddressAggregateRepository.Object);
            _addressAggregateService = new AddressAggregateService(_mockUnitOfWork.Object);
        }
        [Fact]
        public void ServiceShouldReturnAllAddresses()
        {
            //Arrange
            _mockAddressAggregateRepository.Setup(cr => cr.GetAll())
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
            AddressAggregateEntity aggregateEntity = new() {Id = 1};
            //Act
            _addressAggregateService.Delete(aggregateEntity.Id);
            //Assert
            _mockUnitOfWork.Verify(w => w.Commit(), Times.Once);
            _mockAddressAggregateRepository.Verify(r => r.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailOnDeleteAddress()
        {
            //Arrange
            _mockAddressAggregateRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            AddressAggregateEntity aggregateEntity = null;
            
            _addressAggregateService.Invoking(a =>
                a.Delete(aggregateEntity.Id)).Should().Throw<NullReferenceException>();
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
            //Arrange 
            _mockAddressAggregateRepository.Setup(cr
                => cr.Update(It.IsAny<AddressAggregateEntity>()));
            AddressAggregateEntity addressAggregateEntity = new AddressAggregateEntity()
                {Id = 1, Street = "Vardevej 6" };
            
            //Act 
            _addressAggregateService.Update(addressAggregateEntity);
            
            //Assert
            using (new AssertionScope())
            {
                _mockUnitOfWork.Verify(w => w.Commit(), Times.Once);
                _mockAddressAggregateRepository.Verify(r => r.Update(addressAggregateEntity), Times.Once);
            }
        }
        [Fact]
        public void ServiceShouldInsertAddressAggregate()
        {
            AddressAggregateEntity addressAggregateEntity = new AddressAggregateEntity()
            {
                Id = 3,
                CityEntity = new()
                {
                    CityName = "Vejle",
                    Id = 1,
                    PostalCode = "7100"
                },
                CountryEntity = new CountryEntity()
                {
                    CountryCode = "DK",
                    CountryName = "Danmark",
                    Id = 1
                },
                Street = "Langelinje 6",
                
            };
            _mockAddressAggregateRepository.Setup(r => r.Insert(It.IsAny<AddressAggregateEntity>())).Returns(addressAggregateEntity);
            //act
            
            _addressAggregateService.Insert(addressAggregateEntity);
            
            //assert
            using (new AssertionScope())
            {
                _mockUnitOfWork.Verify(w => w.Commit(), Times.Once);
                _mockAddressAggregateRepository.Verify(r => r.Insert(addressAggregateEntity), Times.Once);
            }
        }
        
    }
}