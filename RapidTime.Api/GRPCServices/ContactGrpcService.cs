using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Api.GRPCServices
{
    public class ContactGrpcService(ILogger<ContactGrpcService> logger, IContactService contactService)
        : Contact.ContactBase
    {
        public override Task<ContactResponse> CreateContact(CreateContactRequest request, ServerCallContext context)
        {
            logger.LogInformation("CreateContact called with values {@CreateContact}", request);
            ContactEntity contactEntity = new ContactEntity()
            {
                Email = request.Email,
                Firstname = request.FirstName,
                Lastname = request.LastName,
                TelephoneNumber = request.TelephoneNumber
            };
            var id = contactService.Insert(contactEntity);
            var contact = contactService.FindById(id);
            
            return Task.FromResult( new ContactResponse()
            {
                Email = contact.Email,
                FirstName = contact.Firstname,
                LastName = contact.Lastname,
                TelephoneNumber = contact.TelephoneNumber,
                Id = contact.Id
            });
        }

        public override Task<ContactResponse> AddContactToCustomer(AddContactToCustomerRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get AddContactToCustomer Called with Ids: {Id}", request.CustomerId);
            var contact = contactService.FindById(request.ContactId);
            contactService.AddContactToCustomer(contact, request.CustomerId);
            return Task.FromResult( new ContactResponse()
            {
                Email = contact.Email,
                FirstName = contact.Firstname,
                LastName = contact.Lastname,
                TelephoneNumber = contact.TelephoneNumber
            });
        }

        public override Task<ContactResponse> GetContact(GetContactRequest request, ServerCallContext context)
        {
            logger.LogInformation("Get Contact Called with Id: {Id}", request.Id);
            var contact = contactService.FindById(request.Id);
            return Task.FromResult( new ContactResponse()
            {
                Email = contact.Email,
                FirstName = contact.Firstname,
                LastName = contact.Lastname,
                TelephoneNumber = contact.TelephoneNumber,
                Id = contact.Id
            });
        }

        public override Task<Empty> DeleteContact(DeleteContactRequest request, ServerCallContext context)
        {
            logger.LogInformation("DeleteContact Called with Id: {Id}", request.Id);
            contactService.Delete(request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> UpdateContact(UpdateContactRequest request, ServerCallContext context)
        {
            logger.LogInformation("UpdateContact Called with Id: {Id}", request);
            var contact = contactService.FindById(request.Id);
            contact.Email = request.Email;
            contact.Firstname = request.FirstName;
            contact.Lastname = request.LastName;
            contact.TelephoneNumber = request.TelephoneNumber;
            
            contactService.Update(contact);
            return Task.FromResult(new Empty());
        }

        public override Task<MultiContactResponse> GetAllContacts(Empty request, ServerCallContext context)
        {
            logger.LogInformation("MultiContact called");
            var contacts = contactService.GetAll();

            var responses = new MultiContactResponse();
            responses.Response.Capacity = 0;

            foreach (ContactEntity contact in contacts)
            {
                responses.Response.Add(new ContactResponse()
                {
                    FirstName = contact.Firstname,
                    LastName = contact.Lastname,
                    Email = contact.Email,
                    TelephoneNumber = contact.TelephoneNumber
                });
            }
            return Task.FromResult(responses);
        }
    }
}