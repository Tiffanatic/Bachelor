using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Core.Services;
using RapidTime.Services;
using Xunit;

namespace RapidTime.Tests
{
    public class ContactTests
    {
        public List<ContactEntity> DummyData = new()
        {
            new ContactEntity()
            {
                Id = 1, Firstname = "Mads", Lastname = "Rynord", Email = "test@test.com", TelephoneNumber = "24757727"
            },
            new ContactEntity()
            {
                Id = 2, Firstname = "Jesper", Lastname = "Henriksen", Email = "jenriksen@gmail.com",
                TelephoneNumber = "12345678"
            }
        };

        private Mock<IUnitofWork> _mockUnitOfWork;
        private Mock<IRepository<ContactEntity>> _mockContactRepository;
        private ContactService _contactService;
        private Mock<ICustomerService> _customerService;

        public ContactTests()
        {
            _mockUnitOfWork = new Mock<IUnitofWork>();
            _mockContactRepository = new Mock<IRepository<ContactEntity>>();
            _mockUnitOfWork.Setup(_ => _.ContactRepository).Returns(
                _mockContactRepository.Object);
            _customerService = new Mock<ICustomerService>();
            _contactService = new ContactService(_mockUnitOfWork.Object, _customerService.Object);
        }

        [Fact]
        public void ServiceShouldReturnAllContacts()
        {
            //Arrange
            _mockContactRepository.Setup(cr => cr.GetAll())
                .Returns(DummyData);

            //Act
            var contacts = _contactService.GetAll();

            //Assert
            using (new AssertionScope())
            {
                contacts.Should().HaveCount(2);
                contacts.Should().OnlyHaveUniqueItems(x => x.Id);
                contacts.Should().NotContainNulls();
                contacts.Should().Contain(c => c.Id == 1)
                    .And.Contain(c => c.Id == 2);
            }
        }

        [Fact]
        public void ServiceShouldDeleteContact()
        {
            //Arrange 
            _mockContactRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            ContactEntity contactEntity = new() {Id = 1};
            //Act 
            _contactService.Delete(contactEntity.Id);

            //Assert
            _mockContactRepository.Verify(_ => _.Delete(It.IsAny<int>()), Times.Once);
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
        }

        [Fact]
        public void ServiceShouldFailOnDelete()
        {
            _mockContactRepository.Setup(cr => cr.Delete(It.IsAny<int>()));
            ContactEntity contactEntity = null;

            _contactService.Invoking(a => a.Delete(contactEntity.Id)).Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void ServiceShouldGetById()
        {
            //Arrange
            _mockContactRepository.Setup(cr
                => cr.GetbyId(It.IsAny<int>())).Returns(DummyData[0]);
            //Act
            var contact = _contactService.FindById(1);
            //Assert
            contact.Should().BeEquivalentTo(DummyData[0]);
            
        }

        [Fact]
        public void ServiceShouldUpdateContact()
        {
            //Arrange
            _mockContactRepository.Setup(cr => cr.Update(It.IsAny<ContactEntity>()));
            ContactEntity contactEntity = new ContactEntity() {Id = 1, Firstname = "Jens"};
            //Act
            _contactService.Update(contactEntity);
            //Assert
            _mockUnitOfWork.Verify(_ =>_.Commit(), Times.Once);
            _mockContactRepository.Verify(_ => _.Update(contactEntity), Times.Once);
        }

        [Fact]
        public void ServiceShouldInsertContact()
        {
            ContactEntity contactEntity = new()
            {
                Id = 3,
                Email = "Test2@test.com",
                Firstname = "Martin",
                Lastname = "Fowler",
                TelephoneNumber = "12345678"
            };

            _contactService.Insert(contactEntity);
            
            _mockContactRepository.Verify(_ => _.Insert(contactEntity), Times.Once);
            _mockUnitOfWork.Verify(_ => _.Commit(), Times.Once);
        }
        
        // new Contact()
        // {
        //     Id = 1, Firstname = "Mads", Lastname = "Rynord", Email = "test@test.com", TelephoneNumber = "24757727"
        // },

        [Fact]
        public void ShouldGetProperties()
        {
            var contact = DummyData[0];

            contact.fullName().Should().Be("Mads Rynord");
            contact.Firstname.Should().Be("Mads");
            contact.Lastname.Should().Be("Rynord");
            contact.TelephoneNumber.Should().Be("24757727");
            contact.Email.Should().Be("test@test.com");
        }
    }
}