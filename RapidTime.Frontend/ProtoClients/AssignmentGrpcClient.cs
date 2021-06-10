using System.Collections.Generic;
using System.Linq;
using Grpc.Net.Client;

namespace RapidTime.Frontend.ProtoClients
{
    public class AssignmentGrpcClient
    {
        public List<AssignmentResponse> GetAssignmentsByCustomerId(int customerId)
        {
            var client = GetClient();
            var items = client.GetMultiAssignmentByCustomerId(new MultiAssignmentByCustomerId()
            {
                CustomerId = customerId
            });
            return items.AssignmentResponse.ToList();
        }

        public void DeleteAssignment(int assignmentId)
        {
            var client = GetClient();
            client.DeleteAssignment(new AssignmentRequest()
            {
                Id = assignmentId
            });
        }

        public AssignmentResponse CreateAssignment(CreateAssignmentRequest createAssignmentRequest)
        {
            var client = GetClient();
            return client.CreateAssignment(createAssignmentRequest);
        }

        private Assignment.AssignmentClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Assignment.AssignmentClient(channel);
            return client;
        }
    }
}