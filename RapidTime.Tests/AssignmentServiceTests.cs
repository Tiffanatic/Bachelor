using System;
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
    public class AssignmentServiceTest
    {
        public List<AssignmentEntity> DummyData = new()
        {
            new AssignmentEntity()
            {
                Amount = 10000,
                CustomerEntity = new CustomerEntity() {InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.Dkk},
                Id = 0,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Name = "Revision", InvoiceAble = true},
                DateStarted = DateTime.Now,
                TimeRecords = new List<TimeRecordEntity>()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now,
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new AssignmentEntity()
            {
                Amount = 10000,
                CustomerEntity = new CustomerEntity() {InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.Sek},
                Id = 1,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Name = "Assistance", InvoiceAble = true},
                DateStarted = DateTime.Now,
                TimeRecords = new List<TimeRecordEntity>()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now,
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new AssignmentEntity()
            {
                Amount = 10000,
                CustomerEntity = new CustomerEntity() {InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.Dkk},
                Id = 2,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Name = "Kontor", InvoiceAble = false},
                DateStarted = DateTime.Now,
                TimeRecords = new List<TimeRecordEntity>()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now,
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new AssignmentEntity()
            {
                Amount = 10000,
                CustomerEntity = new CustomerEntity() {InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.Dkk},
                Id = 3,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Name = "Revision", InvoiceAble = true},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecordEntity>()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new AssignmentEntity()
            {
                Amount = 10000,
                CustomerEntity = new CustomerEntity() {InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.Dkk},
                Id = 4,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Name = "Assistance", InvoiceAble = true},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecordEntity>()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new AssignmentEntity()
            {
                Amount = 10000,
                CustomerEntity = new CustomerEntity() {InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.Dkk},
                Id = 5,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Name = "Kontor", InvoiceAble = false},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecordEntity>()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
        };
        
        private readonly Mock<IUnitofWork> _mockUnitOfWork;
        private readonly Mock<IAssignmentRepository> _mockAssignmentRepository;
        private readonly AssignmentService _assignmentService;
        private readonly Mock<ILogger<AssignmentService>> _mocklogger;
        
        
        public AssignmentServiceTest()
        {
            _mockAssignmentRepository = new Mock<IAssignmentRepository>();
            _mocklogger = new Mock<ILogger<AssignmentService>>();
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _assignmentService = new AssignmentService(_mockUnitOfWork.Object, _mocklogger.Object);

            _mockUnitOfWork.Setup(x => x.AssignmentRepository).Returns(_mockAssignmentRepository.Object);
        }

        [Fact]
        public void ServiceShouldReturnAllAssignments()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.GetAll()).Returns(DummyData);

            //Act
            var assignments = _assignmentService.GetAll();

            //Assert

            assignments.Should().HaveCount(6);
            assignments.Should().OnlyHaveUniqueItems(p => p.Id);
            assignments.Should().NotContainNulls();
            assignments.Should().Contain(p => p.Id == 0)
                .And.Contain(p => p.Id == 1)
                .And.Contain(p => p.Id == 2)
                .And.Contain(p => p.Id == 3)
                .And.Contain(p => p.Id == 4)
                .And.Contain(p => p.Id == 5);

        }

        [Fact]
        public void ServiceShouldReturnASpecificAssignment()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.GetbyId(It.IsAny<int>())).Returns(DummyData[1]);
            
            //Act
            var assignment = _assignmentService.GetById(1);
            
            //Assert
            assignment.Should().BeEquivalentTo(DummyData[1]);
        }

        [Fact]
        public void ServiceShouldDeleteAssignment()
        {
            //Arrange
            _mockAssignmentRepository.Setup(repository => repository.Delete(It.IsAny<int>()));

            //Act
            _assignmentService.Delete(DummyData[0].Id);

            //Assert
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
            _mockAssignmentRepository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailToDeleteWrongId()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.Delete(It.IsAny<int>()));

            AssignmentEntity assignmentEntity = null;
            
            //Act & Assert
            _assignmentService.Invoking(ar => ar.Delete(assignmentEntity.Id)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceShouldInsertAssignment()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.Insert(It.IsAny<AssignmentEntity>()));
            
            AssignmentEntity assignmentEntity = new AssignmentEntity()
            {
                Amount = 10000,
                CustomerEntity = new CustomerEntity() {InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.Dkk},
                Id = 6,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Name = "Kontor", InvoiceAble = false},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecordEntity>()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)

                    }
                }
            };

            _mockAssignmentRepository.Setup(r => r.Insert(It.IsAny<AssignmentEntity>())).Returns(assignmentEntity);
            //Act
            
            _assignmentService.Insert(assignmentEntity);
            
            //Assert
            _mockAssignmentRepository.Verify(x => x.Insert(assignmentEntity), Times.Once);
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldUpdateAssignment()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.Update(It.IsAny<AssignmentEntity>()));
            
            //Act
            AssignmentEntity assignmentEntity = new AssignmentEntity()
            {
                Amount = 10000,
                CustomerEntity = new CustomerEntity() {InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.Dkk},
                Id = 5,
                AssignmentTypeEntity = new AssignmentTypeEntity() {Name = "Kontor", InvoiceAble = false},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecordEntity>()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)

                    }
                }
            };
            
            _assignmentService.Update(assignmentEntity);
            
            //Assert
            _mockAssignmentRepository.Verify(x => x.Update(assignmentEntity), Times.Once);
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
        }
    }
}