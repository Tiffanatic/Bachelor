using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class PriceServiceTest
    {
        public List<Price> DummyData = new()
        {
            new Price()
            {
                Id = 0,
                AssignmentType =
                    new AssignmentType() {Id = 0, Name = "Revision", Number = "100101", InvoiceAble = true},
                HourlyRate = 1600
            },
            new Price()
            {
                Id = 1,
                AssignmentType = new AssignmentType()
                    {Id = 1, Name = "Assistance", Number = "100104", InvoiceAble = true},
                HourlyRate = 1500
            },
            new Price()
            {
                Id = 2,
                AssignmentType = new AssignmentType() {Id = 2, Name = "Kontor", Number = "100101", InvoiceAble = false},
                HourlyRate = 0
            },
            new Price()
            {
                Id = 3,
                AssignmentType = new AssignmentType() {Id = 3, Name = "Kurser", Number = "100101", InvoiceAble = false},
                HourlyRate = 0
            }
        };

        private readonly Mock<IUnitofWork> _mockUnitOfWork;
        private Mock<IRepository<Price>> _mockPriceRepository;
        private PriceService _priceService;
        private Mock<ILogger> _logger;

        public PriceServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _mockPriceRepository = new Mock<IRepository<Price>>();
            _logger = new Mock<ILogger>();
            _priceService = new PriceService(_mockUnitOfWork.Object, _logger.Object);


            _mockUnitOfWork.Setup(_ => _.PriceRepository).Returns(_mockPriceRepository.Object);
        }

        [Fact]
        public void ServiceShouldReturnAllPrices()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.getAll()).Returns(DummyData);

            //Act
            var prices = _priceService.GetAll();

            //Assert

            prices.Should().HaveCount(4);
            prices.Should().OnlyHaveUniqueItems(p => p.Id);
            prices.Should().NotContainNulls();
            prices.Should().Contain(p => p.Id == 0)
                .And.Contain(p => p.Id == 1)
                .And.Contain(p => p.Id == 2)
                .And.Contain(p => p.Id == 3);
        }

        [Fact]
        public void ServiceShouldReturnASpecificPriceWithHourlyRate1600()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.GetbyId(0)).Returns(DummyData[0]);

            //Act
            var prices = _priceService.GetById(0);

            //Assert
            prices.Should().Be(DummyData[0]);
            prices.HourlyRate.Should().Be(1600);
            prices.Should().BeEquivalentTo(DummyData[0]);
        }

        [Fact]
        public void ServiceShouldReturnHourlyRate()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.GetbyId(1)).Returns(DummyData[1]);

            //Act
            var prices = _priceService.GetById(1);

            //Assert
            prices.HourlyRate.Should().Be(1500);
        }

        [Fact]
        public void ServiceShouldDeletePrice()
        {
            //Arrange
            _mockPriceRepository.Setup(repository => repository.Delete(It.IsAny<int>()));

            //Act
            _priceService.Delete(DummyData[3].Id);

            //Arrange
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            _mockPriceRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailToUpdatePrice()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.PriceRepository).Returns(_mockPriceRepository.Object);
            Price price = null;

            //Act & Assert
            _priceService.Invoking(ps => ps.Delete(price.Id)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceShouldUpdatePrice()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.Update(It.IsAny<Price>()));
            Price price = new Price()
            {
                Id = 0,
                AssignmentType =
                    new AssignmentType() {Id = 0, Name = "Revision", Number = "100101", InvoiceAble = true},
                HourlyRate = 2000
            };
            
            //Act
            _priceService.Update(price);
            
            //Assert
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            _mockPriceRepository.Verify(x => x.Update(price), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailToDeletePriceWithoutId()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.Delete(It.IsAny<int>()));

            Price price = null;
            
            //Act & Assert
            _priceService.Invoking(ps => ps.Delete(price.Id)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceShouldInsertNewPrice()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.Insert(It.IsAny<Price>()));
            
            
            //Act
            Price price = new Price()
            {
                Id = 5,
                AssignmentType = null,
                HourlyRate = 100
            };
            
            _priceService.Insert(price);
            
            //Assert
            _mockPriceRepository.Verify(x => x.Insert(price), Times.Once);
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
        }
    }
}