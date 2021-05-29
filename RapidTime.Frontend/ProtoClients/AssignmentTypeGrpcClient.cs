using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

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
    }
}