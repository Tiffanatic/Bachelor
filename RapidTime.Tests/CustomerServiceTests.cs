using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class CustomerServiceTests
    {

        public List<CustomerEntity> DummyData = new List<CustomerEntity>()
        {
            new()
            {
                Id = 1,
                Address = new()
                {
                    Id = 1,
                    CityEntity = new()
                    {
                        CityName = "Vejle",
                        Id = 1,
                        PostalCode = "7100"
                    }
                },
                CompanyTypeEntity = new CompanyTypeEntity() {Id = 1, CompanyTypeName = "A/S"},
                
                InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.DKK,
                InvoiceMail = "Economy@test.com",
                Name = "Test company",
                YearlyReview = new DateTime(1970, 1, 1)
            },
            new()
            {
                Id = 2,
                Address = new()
                {
                    Id = 1,
                    CityEntity = new()
                    {
                        CityName = "Vejle",
                        Id = 1,
                        PostalCode = "7100"
                    }
                },
                CompanyTypeEntity = new CompanyTypeEntity() {Id = 1, CompanyTypeName = "A/S"},
                
                InvoiceCurrency = CustomerEntity.InvoiceCurrencyEnum.DKK,
                InvoiceMail = "Economy@test.com",
                Name = "Test holding company",
                YearlyReview = new DateTime(1970, 1, 1)
            }
        };

        private Mock<IUnitofWork> _mockUnitOfWork;
        private Mock<IRepository<CustomerEntity>> _mockCustomerReposity;
        private CustomerService _customerService;
        
        
        public CustomerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _mockCustomerReposity = new Mock<IRepository<CustomerEntity>>();
            _mockUnitOfWork.Setup(_ => _.CustomerRepository).Returns(
                _mockCustomerReposity.Object);
            _customerService = new CustomerService(_mockUnitOfWork.Object, new Mock<ILogger>().Object);
        }

        [Fact]
        public void ServiceShouldGetAllCustomers()
        {
            //Arrange 
            _mockCustomerReposity.Setup(cr
                => cr.GetAll()).Returns(DummyData);
            
            //Act 

            var customers = _customerService.GetAllCustomers();
            
            //Assert
            using (new AssertionScope())
            {
                customers.Should().HaveCount(2);
                customers.Should().OnlyHaveUniqueItems(x => x.Id);
                customers.Should().NotContainNulls();
                customers.Should().Contain(c => c.Id == 1)
                    .And.Contain(c => c.Id == 2);
            }
        }

        [Fact]
        public void ServiceShouldDelete()
        {
            //Arrange
            _mockCustomerReposity.Setup(cr
                => cr.Delete(It.IsAny<int>()));
            CustomerEntity customerEntity = new CustomerEntity() {Id = 1};
            //Act
            _customerService.Delete(customerEntity.Id);
            
            //Assert
            _mockCustomerReposity.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldThrowOnDelete()
        {
            _mockCustomerReposity.Setup(cr
                => cr.Delete(It.IsAny<int>()));
            CustomerEntity customerEntity = null;
            //Act
            _customerService.Invoking(x => x.Delete(customerEntity.Id)).Should().Throw<NullReferenceException>();
            
        }

        [Fact]
        public void ServiceShouldInsert()
        {
            //Arrange
            _mockCustomerReposity.Setup(cr
                => cr.Insert(It.IsAny<CustomerEntity>()));

            CustomerEntity customerEntity = new CustomerEntity() {Id = 3};
            
            //Act
            _customerService.Insert(customerEntity);

            //Assert
            _mockCustomerReposity.Verify(_ => _.Insert(It.IsAny<CustomerEntity>()), Times.Once);
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldUpdate()
        {
            //Arrange
            _mockCustomerReposity.Setup(cr
                => cr.Update(It.IsAny<CustomerEntity>()));

            CustomerEntity customerEntity = new() {Id = 1};
            
            //Act
            _customerService.Update(customerEntity);
            
            //Assert
            _mockCustomerReposity.Verify(_ => _.Update(It.IsAny<CustomerEntity>()), Times.Once);
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldGetById()
        {
            //Arrange
            _mockCustomerReposity.Setup(cr
                => cr.GetbyId(It.IsAny<int>())).Returns(DummyData[0]);

            //Act
            CustomerEntity customerEntity = _customerService.GetById(1);
            
            //Assert
            customerEntity.Should().BeEquivalentTo(DummyData[0]);

        }

        [Theory]
        [InlineData("Test company", 1)]
        [InlineData("holding", 1)]
        [InlineData("Test", 2)]
        public void ServiceShouldFindCustomerByName(string input, int amountExpectedReturn)
        {
            //Arrange
            _mockCustomerReposity.Setup(cr
                => cr.GetAll()).Returns(DummyData);
            
            //Act
            CustomerEntity[] customers = _customerService.FindByName(input);
            
            //Assert
            customers.Should().HaveCount(amountExpectedReturn);
            customers.Should().OnlyHaveUniqueItems(x => x.Id);
            
        }
    }
}