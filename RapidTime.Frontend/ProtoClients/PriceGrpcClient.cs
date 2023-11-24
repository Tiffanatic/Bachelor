using Grpc.Net.Client;
using System.Collections.Generic;
using System.Linq;
using RapidTime.Frontend.Models;

namespace RapidTime.Frontend.ProtoClients
{
    public class PriceGrpcClient
    {

       
        public PriceGrpcClient()
        {
            
        }

        private Price.PriceClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Price.PriceClient(channel);
            return client;
        }

        public List<PriceResponse> GetPricesForUser(string id)
        {
            var client = GetClient();
            var prices = client.UserPrices(new GetUserPriceListRequest()
            {
                Id = id
            });

            return prices.Response.ToList();

        }

        public void CreatePrice(CreatePriceResource createPriceResource)
        {
            var client = GetClient();
            client.CreatePrice(new CreatePriceRequest()
            {
                AssignmentType = createPriceResource.AssignmentType,
                HourlyRate = createPriceResource.HourlyRate,
                UserId = createPriceResource.Id
            });
        }
    }
}
