using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using RapidTime.Frontend.Models;

namespace RapidTime.Frontend.ProtoClients
{
    public class AssignmentTypeGrpcClient
    {
        private AssignmentType.AssignmentTypeClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new AssignmentType.AssignmentTypeClient(channel);
            return client;
        }

        public List<AssignmentTypeResponse> GetAssignmentTypes()
        {
            var client = GetClient();
            return client.GetAllAssignmentType(new Empty()).AssignmentType.ToList();
        }

        public void CreateAssignmentType(CreateAssignmentTypeResource createAssignmentTypeResource)
        {
            var client = GetClient();
            var res = client.CreateAssignmentType(new CreateAssignmentTypeRequest()
            {
                InvoiceAble = createAssignmentTypeResource.InvoiceAble,
                Name = createAssignmentTypeResource.Name,
                Number = createAssignmentTypeResource.Number
            });
        }

        public void DeleteAssignmentType(int id)
        {
            var client = GetClient();
            var res = client.DeleteAssignmentType(new DeleteAssignmentTypeRequest()
            {
                Id = id
            });
        }

        public AssignmentTypeResponse GetAssignmentById(int id)
        {
            var client = GetClient();
            var response = client.GetAssignmentType(new GetAssignmentTypeRequest()
            {
                Id = id
            });
            return response;
        }
        
    }
}