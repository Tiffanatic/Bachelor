using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Services;

namespace RapidTime.Api.GRPCServices
{
    public class ContactGrpcService : Contact.ContactBase
    {
        private ILogger<ContactGrpcService> _logger;
        private IContactService _contactService;

        public ContactGrpcService(ILogger<ContactGrpcService> logger, IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        public override Task<ContactResponse> CreateContact(CreateContactRequest request, ServerCallContext context)
        {
            _logger.LogInformation("CreateContact called with values {@CreateContact}", request);
            ContactEntity contactEntity = new ContactEntity()
            {
                Email = request.Email,
                Firstname = request.FirstName,
                Lastname = request.LastName,
                TelephoneNumber = request.TelephoneNumber
            };
            var id = _contactService.Insert(contactEntity);
            var contact = _contactService.FindById(id);
            
            return Task.FromResult( new ContactResponse()
            {
                Response = new()
                {
                    Email = contact.Email,
                    FirstName = contact.Firstname,
                    LastName = contact.Lastname,
                    TelephoneNumber = contact.TelephoneNumber,
                    Id = contact.Id
                }
            });
        }

        public override Task<ContactResponse> AddContactToCustomer(AddContactToCustomerRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Get AddContactToCustomer Called with Ids: {Id}, {Id}", request.ContactId, request.CustomerId);
            var contact = _contactService.FindById(request.ContactId);
            _contactService.AddContactToCustomer(contact, request.CustomerId);
            return Task.FromResult( new ContactResponse()
            {
                Response =
                {
                    Email = contact.Email,
                    FirstName = contact.Firstname,
                    LastName = contact.Lastname,
                    TelephoneNumber = contact.TelephoneNumber
                }
            });
        }

        public override Task<ContactResponse> GetContact(GetContactRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Get Contact Called with Id: {Id}", request.Id);
            var contact = _contactService.FindById(request.Id);
            return Task.FromResult( new ContactResponse()
            {
                Response = new()
                {
                    Email = contact.Email,
                    FirstName = contact.Firstname,
                    LastName = contact.Lastname,
                    TelephoneNumber = contact.TelephoneNumber,
                    Id = contact.Id
                }
            });
        }

        public override Task<Empty> DeleteContact(DeleteContactRequest request, ServerCallContext context)
        {
            _logger.LogInformation("DeleteContact Called with Id: {Id}", request.Id);
            _contactService.Delete(request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> UpdateContact(UpdateContactRequest request, ServerCallContext context)
        {
            _logger.LogInformation("UpdateContact Called with Id: {Id}", request);
            var contact = _contactService.FindById(request.UpdatedContact.Id);
            contact.Email = request.UpdatedContact.Email;
            contact.Firstname = request.UpdatedContact.FirstName;
            contact.Lastname = request.UpdatedContact.LastName;
            contact.TelephoneNumber = request.UpdatedContact.TelephoneNumber;
            
            _contactService.Update(contact);
            return Task.FromResult(new Empty());
        }

        public override Task<MultiContactResponse> GetAllContacts(Empty request, ServerCallContext context)
        {
            _logger.LogInformation("MultiContact called");
            var contacts = _contactService.GetAll();
            var response = new MultiContactResponse()
            {
                Response = { }
            };

            var responses = new MultiContactResponse()
            {
                Response = { }
            };
            
            foreach (ContactEntity contact in contacts)
            {
                responses.Response.Add(new ContactBase()
                {
                    FirstName = contact.Firstname,
                    LastName = contact.Lastname,
                    Email = contact.Email,
                    TelephoneNumber = contact.TelephoneNumber
                });
            }
            return Task.FromResult<MultiContactResponse>(responses);
        }
    }
}