using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Models.Address;
using RapidTime.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class TimeRegistrationServiceTests
    {
        public List<AssignmentEntity> DummyData = new()
        {
            new AssignmentEntity()
            {
                Id = 0,
                DateStarted = DateTime.Now.AddDays(-4),
                TimeRecords = new()
                {
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now,
                        TimeRecorded = new TimeSpan(0, 20, 0)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Now,
                        TimeRecorded = new TimeSpan(0, 20, 0)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Today.AddDays(-1),
                        TimeRecorded = new TimeSpan(0, 55, 0)
                    },
                    new TimeRecordEntity()
                    {
                        Date = DateTime.Today.AddDays(-3),
                        TimeRecorded = new TimeSpan(0, 55, 0)
                    }
                }
            }
        };
        
        private Mock<IUnitofWork> _mockUnitOfWork;
        private Mock<ILogger> _logger;
        private Mock<IRepository<AssignmentEntity>> _mockAssignmentRepository;
        private TimeRegistrationService _timeRegistrationService;
        private Mock<IRepository<TimeRecordEntity>> _mockTimeRecordRepository;
        private  AssignmentService _assignmentService;

        public TimeRegistrationServiceTests()
        {
            _logger = new Mock<ILogger>();
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _mockAssignmentRepository = new Mock<IRepository<AssignmentEntity>>();
            _mockTimeRecordRepository = new Mock<IRepository<TimeRecordEntity>>();
            _assignmentService = new AssignmentService(_mockUnitOfWork.Object, _logger.Object);
            _timeRegistrationService = new TimeRegistrationService(_mockUnitOfWork.Object, _assignmentService, _logger.Object);

            _mockUnitOfWork.Setup(_ => _.AssignmentRepository).Returns(_mockAssignmentRepository.Object);

        }
        
        [Fact]
        public void ServiceShouldReturnCorrectTimeRegistered()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.GetbyId(0)).Returns(DummyData[0]);
            
            //Act
            var timeRecordedForAssignment = _timeRegistrationService.GetTimeRecordedForAssignment(0);

            //Assert
            timeRecordedForAssignment.Should().Be(new TimeSpan(0, 2, 30, 0));
            timeRecordedForAssignment.Should().Be(new TimeSpan(0, 0, 150, 0));
        }

        [Fact]
        public void ServiceShouldFailToRegisterMoreThan24HoursADayPerUser()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.GetAll()).Returns(DummyData);
            _mockAssignmentRepository.Setup(ar => ar.GetbyId(0)).Returns(DummyData[0]);
            
            //Act
            TimeRecordEntity tooBigToRegister = new TimeRecordEntity() {Date = DateTime.Now, TimeRecorded = new TimeSpan(25, 0, 0)};
            var register25Hours = _timeRegistrationService.RegisterTime(tooBigToRegister, 0);
            
            //Assert
            register25Hours.Should().Be(false);
        }

        [Fact]
        public void ServiceShouldReturnRegisteredTimeOnAssignmentBetween2Dates()
        {
            //Arrange
            _mockAssignmentRepository.Setup(ar => ar.GetbyId(0)).Returns(DummyData[0]);

            //Act
            TimeSpan registeredTime1 =
                _timeRegistrationService.GetTimeRegisteredBetweenDates(0,DateTime.Now.AddDays(-2), DateTime.Now);
            TimeSpan registeredTime2 = _timeRegistrationService.GetTimeRegisteredBetweenDates(0, DateTime.Now.AddDays(-4), DateTime.Now);

            //Assert
            registeredTime1.Should().Be(new TimeSpan(1, 35, 0));
            registeredTime2.Should().Be(new TimeSpan(2, 30, 0));

            registeredTime1.Should().Be(95.Minutes());
            registeredTime2.Should().Be(150.Minutes());

            registeredTime1.Should().BePositive();
            registeredTime2.Should().BePositive();
        }
    }
}