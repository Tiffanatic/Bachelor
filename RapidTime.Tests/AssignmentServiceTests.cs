using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Services;
using RapidTime.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class AssignmentServiceTest
    {
        public List<Assignment> DummyData = new()
        {
            new Assignment()
            {
                Amount = 10000,
                Customer = new Customer() {InvoiceCurrency = Customer.InvoiceCurrencyEnum.DKK},
                Id = 0,
                AssignmentType = new AssignmentType() {Name = "Revision", InvoiceAble = true},
                DateStarted = DateTime.Now,
                TimeRecords = new List<TimeRecord>()
                {
                    new TimeRecord()
                    {
                        Date = DateTime.Now,
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new Assignment()
            {
                Amount = 10000,
                Customer = new Customer() {InvoiceCurrency = Customer.InvoiceCurrencyEnum.SEK},
                Id = 1,
                AssignmentType = new AssignmentType() {Name = "Assistance", InvoiceAble = true},
                DateStarted = DateTime.Now,
                TimeRecords = new List<TimeRecord>()
                {
                    new TimeRecord()
                    {
                        Date = DateTime.Now,
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new Assignment()
            {
                Amount = 10000,
                Customer = new Customer() {InvoiceCurrency = Customer.InvoiceCurrencyEnum.DKK},
                Id = 2,
                AssignmentType = new AssignmentType() {Name = "Kontor", InvoiceAble = false},
                DateStarted = DateTime.Now,
                TimeRecords = new List<TimeRecord>()
                {
                    new TimeRecord()
                    {
                        Date = DateTime.Now,
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddDays(-1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new Assignment()
            {
                Amount = 10000,
                Customer = new Customer() {InvoiceCurrency = Customer.InvoiceCurrencyEnum.DKK},
                Id = 3,
                AssignmentType = new AssignmentType() {Name = "Revision", InvoiceAble = true},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecord>()
                {
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new Assignment()
            {
                Amount = 10000,
                Customer = new Customer() {InvoiceCurrency = Customer.InvoiceCurrencyEnum.DKK},
                Id = 4,
                AssignmentType = new AssignmentType() {Name = "Assistance", InvoiceAble = true},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecord>()
                {
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
            new Assignment()
            {
                Amount = 10000,
                Customer = new Customer() {InvoiceCurrency = Customer.InvoiceCurrencyEnum.DKK},
                Id = 5,
                AssignmentType = new AssignmentType() {Name = "Kontor", InvoiceAble = false},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecord>()
                {
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                        
                    }
                }
            },
        };
        
        private readonly Mock<IUnitofWork> _mockUnitOfWork;
        private Mock<IRepository<Assignment>> _mockAssignmentRepository;
        private AssignmentService _assignmentService;
        private Mock<ILogger> _logger;
        
        
        public AssignmentServiceTest()
        {
            _mockAssignmentRepository = new Mock<IRepository<Assignment>>();
            _logger = new Mock<ILogger>();
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _assignmentService = new AssignmentService(_mockUnitOfWork.Object, _logger.Object);

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

            Assignment assignment = null;
            
            //Act & Assert
            _assignmentService.Invoking(ar => ar.Delete(assignment.Id)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceShouldInsertAssignment()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.Insert(It.IsAny<Assignment>()));
            
            //Act
            Assignment assignment = new Assignment()
            {
                Amount = 10000,
                Customer = new Customer() {InvoiceCurrency = Customer.InvoiceCurrencyEnum.DKK},
                Id = 6,
                AssignmentType = new AssignmentType() {Name = "Kontor", InvoiceAble = false},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecord>()
                {
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)

                    }
                }
            };
            
            _assignmentService.Insert(assignment);
            
            //Assert
            _mockAssignmentRepository.Verify(x => x.Insert(assignment), Times.Once);
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldUpdateAssignment()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.Update(It.IsAny<Assignment>()));
            
            //Act
            Assignment assignment = new Assignment()
            {
                Amount = 10000,
                Customer = new Customer() {InvoiceCurrency = Customer.InvoiceCurrencyEnum.DKK},
                Id = 5,
                AssignmentType = new AssignmentType() {Name = "Kontor", InvoiceAble = false},
                DateStarted = DateTime.Now.AddYears(-2),
                TimeRecords = new List<TimeRecord>()
                {
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(1),
                        TimeRecorded = TimeSpan.FromMinutes(100)
                    },
                    new TimeRecord()
                    {
                        Date = DateTime.Now.AddYears(-2).AddDays(2),
                        TimeRecorded = TimeSpan.FromMinutes(100)

                    }
                }
            };
            
            _assignmentService.Update(assignment);
            
            //Assert
            _mockAssignmentRepository.Verify(x => x.Update(assignment), Times.Once);
            _mockUnitOfWork.Verify(x => x.Commit(), Times.Once);
        }
    }
}