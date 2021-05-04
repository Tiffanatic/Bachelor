using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class AssignmentTypeServiceTests
    {
        public List<AssignmentType> DummyData = new()
        {
            new() {Id = 1, Name = "Revision", Number = "001", InvoiceAble = true},
            new() {Id = 2, Name = "Consulting", Number = "002", InvoiceAble = true}
        };

        private Mock<IUnitofWork> _mockUnitWork;
        private Mock<IRepository<AssignmentType>> _mockAssignmentTypeRepository;
        private AssignmentTypeService _assignmentTypeService;
        
        public AssignmentTypeServiceTests()
        {
            _mockUnitWork = new Mock<IUnitofWork>();
            _mockAssignmentTypeRepository = new Mock<IRepository<AssignmentType>>();
            _mockUnitWork.Setup(_ => _.AssignmentTypeRepository).Returns(
                _mockAssignmentTypeRepository.Object);

            _assignmentTypeService = new AssignmentTypeService(_mockUnitWork.Object);
        }

        [Fact]
        public void ServiceShouldReturnAllAssignmentTypes()
        {
            //Arrange 
            _mockAssignmentTypeRepository.Setup(cr 
                => cr.GetAll()).Returns(DummyData);
            //Act 
            var assignmentTypes = _assignmentTypeService.GetAll();
            //Assert
            assignmentTypes.Should().HaveCount(2);
            assignmentTypes.Should().OnlyHaveUniqueItems(c => c.Id);
            assignmentTypes.Should().NotContainNulls();
            assignmentTypes.Should().Contain(c => c.Id == 1)
                .And.Contain(c => c.Id == 2);
        }

        [Fact]
        public void ServiceShouldDeleteAssignmentType()
        {
            //Arrange
            _mockAssignmentTypeRepository.Setup(cr
                => cr.Delete(It.IsAny<int>()));
            AssignmentType assignmentType = new()
            {
                Id = 1
            };
            
            //Act
            _assignmentTypeService.Delete(assignmentType.Id);
            
            //Assert
            _mockUnitWork.Verify(_ => _.Commit(), Times.Once);
            _mockAssignmentTypeRepository.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
        }
        [Fact]
        public void ServiceShouldThrowOnDeleteAssignmentType()
        {
            //Arrange
            _mockAssignmentTypeRepository.Setup(cr
                => cr.Delete(It.IsAny<int>()));
            AssignmentType assignmentType = null;
            
            //Act
            _assignmentTypeService.Invoking(_ => _.Delete(assignmentType.Id))
                .Should().Throw<NullReferenceException>();
        }

        [Theory]
        [InlineData("Revision")]
        [InlineData("Revi")]
        [InlineData("002")]
        [InlineData("Consul")]
        public void ServiceShouldGetByNameOrNumber(string input)
        {
            //Arramge
            _mockAssignmentTypeRepository.Setup(_ => _.GetAll()).Returns(DummyData);
            //Act
            var result = _assignmentTypeService.GetByNameOrNumber(input);
            
            //Assert
            result.Should().NotBeEmpty();
            result.Should().NotContainNulls();
            result.Should().OnlyHaveUniqueItems(c => c.Id);
        }

        [Fact]
        public void ServiceShouldInsertAssignmentTypeById()
        {
            //Arrange
            _mockAssignmentTypeRepository.Setup(cr =>
                cr.Insert(It.IsAny<AssignmentType>()));

            AssignmentType assignmentType = new()
            {
                Id = 3
            };
            
            //Act
            _assignmentTypeService.Insert(assignmentType);
            
            //Assert
            _mockUnitWork.Verify(_ => _.Commit(), Times.Once);
            _mockAssignmentTypeRepository.Verify(_ => _.Insert(assignmentType), Times.Once);
        }

        [Fact]
        public void ServiceShouldGetById()
        {
            //Arrange
            _mockAssignmentTypeRepository.Setup(_
                => _.GetbyId(It.IsAny<int>())).Returns(DummyData[0]);
            //Act
            var assignmentType =_assignmentTypeService.GetById(1);
            //Assert
            assignmentType.Should().BeEquivalentTo(DummyData[0]);
        }

        [Fact]
        public void ServiceShouldUpdateAssignmentType()
        {
            AssignmentType assignmentType = new() {Id = 1, Name = "Bookkeeping"};

            _assignmentTypeService.Update(assignmentType);
        }

        [Fact]
        public void ServiceShouldFailOnUpdate()
        {
            AssignmentType assignmentType = null; 
            
            _assignmentTypeService.Invoking(_ 
                => _.Update(assignmentType)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void PropertyTest()
        {
            DummyData[0].InvoiceAble.Should().Be(true);
        }
    }
}