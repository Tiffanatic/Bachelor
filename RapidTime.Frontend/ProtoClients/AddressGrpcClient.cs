using Grpc.Net.Client;
using RapidTime.Frontend.Models;

namespace RapidTime.Frontend.ProtoClients
{
    public class AddressGrpcClient
    {
        public AddressGrpcClient()
        {
            
        }

        public void CreateAddressOnCustomer(CreateAddressResource createAddressResource, int customerId)
        {
            
        }
        
        
        private AddressAggregate.AddressAggregateClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new AddressAggregate.AddressAggregateClient(channel);
            return client;
        }
        
        
        
    }
}