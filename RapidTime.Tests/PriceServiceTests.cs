﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Repositories;
using RapidTime.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class PriceServiceTest
    {
        public List<PriceEntity> DummyData = new()
        {
            new PriceEntity()
            {
                Id = 0,
                AssignmentTypeEntity =
                    new AssignmentTypeEntity() {Id = 0, Name = "Revision", Number = "100101", InvoiceAble = true},
                HourlyRate = 1600
            },
            new PriceEntity()
            {
                Id = 1,
                AssignmentTypeEntity = new AssignmentTypeEntity()
                    {Id = 1, Name = "Assistance", Number = "100104", InvoiceAble = true},
                HourlyRate = 1500
            },
            new PriceEntity()
            {
                Id = 2,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Id = 2, Name = "Kontor", Number = "100101", InvoiceAble = false},
                HourlyRate = 0
            },
            new PriceEntity()
            {
                Id = 3,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Id = 3, Name = "Kurser", Number = "100101", InvoiceAble = false},
                HourlyRate = 0
            }
        };

        private readonly Mock<IUnitofWork> _mockUnitOfWork;
        private readonly Mock<IRepository<PriceEntity>> _mockPriceRepository;
        private readonly PriceService _priceService;
        private readonly Mock<ILogger<PriceService>> _logger;

        public PriceServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _mockPriceRepository = new Mock<IRepository<PriceEntity>>();
            _logger = new Mock<ILogger<PriceService>>();
            _priceService = new PriceService(_mockUnitOfWork.Object, _logger.Object);


            _mockUnitOfWork.Setup(w => w.PriceRepository).Returns(_mockPriceRepository.Object);
        }

        [Fact]
        public void ServiceShouldReturnAllPrices()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.GetAll()).Returns(DummyData);

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

            //Assert
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            _mockPriceRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailToUpdatePrice()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.PriceRepository).Returns(_mockPriceRepository.Object);
            PriceEntity priceEntity = null;

            //Act & Assert
            _priceService.Invoking(ps => ps.Delete(priceEntity.Id)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceShouldUpdatePrice()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.Update(It.IsAny<PriceEntity>()));
            PriceEntity priceEntity = new PriceEntity()
            {
                Id = 0,
                AssignmentTypeEntity =
                    new AssignmentTypeEntity() {Id = 0, Name = "Revision", Number = "100101", InvoiceAble = true},
                HourlyRate = 2000
            };
            
            //Act
            _priceService.Update(priceEntity);
            
            //Assert
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            _mockPriceRepository.Verify(x => x.Update(priceEntity), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailToDeletePriceWithoutId()
        {
            //Arrange
            _mockPriceRepository.Setup(pr => pr.Delete(It.IsAny<int>()));

            PriceEntity priceEntity = null;
            
            //Act & Assert
            _priceService.Invoking(ps => ps.Delete(priceEntity.Id)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceShouldInsertNewPrice()
        {
            //Arrange
            
            PriceEntity priceEntity = new PriceEntity()
            {
                Id = 5,
                AssignmentTypeEntity = null,
                HourlyRate = 100
            };
            _mockPriceRepository.Setup(pr => pr.Insert(It.IsAny<PriceEntity>())).Returns(priceEntity);
            
            //Act
            
            _priceService.Insert(priceEntity);
            
            //Assert
            _mockPriceRepository.Verify(x => x.Insert(priceEntity), Times.Once);
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
        }
    }
}