using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using RapidTime.Frontend.Models;

namespace RapidTime.Frontend.ProtoClients
{
    public class ContactGrpcClient
    {
        public Task<ContactResponse> CreateContact(CreateContactResource contactResourceToCreate)
        {
            Contact.ContactClient client = GetClient();
            var res = client.CreateContact(new CreateContactRequest()
            {
                FirstName = contactResourceToCreate.FirstName,
                LastName = contactResourceToCreate.LastName,
                TelephoneNumber = contactResourceToCreate.Telephonenumber,
                Email = contactResourceToCreate.Email
            });

            return Task.FromResult(res);
        }

        public Empty UpdateContact(UpdateContactRequest updateContactRequest)
        {
            Contact.ContactClient client = GetClient();
            Empty res = client.UpdateContact(new UpdateContactRequest
            {
                UpdatedContact = updateContactRequest.UpdatedContact
            });
        
            return new Empty();
        }

        public ContactResponse GetContact(int contactId)
        {
            Contact.ContactClient client = GetClient();
            ContactResponse res = client.GetContact(new GetContactRequest
            {
                Id = contactId
            });

            return res;
        }

        public Empty DeleteContact(int contactId)
        {
            Contact.ContactClient client = GetClient();
            client.DeleteContact(new DeleteContactRequest
            {
                Id = contactId
            });

            return new Empty();
        }

        public ContactResponse AddContactToCustomer(AddContactToCustomerRequest addContactToCustomerRequest)
        {
            Contact.ContactClient client = GetClient();
            client.AddContactToCustomer(new AddContactToCustomerRequest
            {
                CustomerId = addContactToCustomerRequest.CustomerId,
                ContactId = addContactToCustomerRequest.ContactId
            });

            ContactResponse res = client.GetContact(new GetContactRequest
            {
                Id = addContactToCustomerRequest.ContactId
            });

            return res;
        }

        public List<ContactBase> GetAllContacts()
        {
            var client = GetClient();
            var contacts = client.GetAllContacts(new Google.Protobuf.WellKnownTypes.Empty());

            List<ContactBase> contactBases = new List<ContactBase>();
            foreach (var contact in contacts.Response)
            {
                contactBases.Add(contact);
            }

            return contactBases;
        }

        private Contact.ContactClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Contact.ContactClient(channel);
            return client;
        }
    }
}