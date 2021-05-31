using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using RapidTime.Frontend.Models;
using RapidTime.Frontend.Pages.PriceComponents;

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
                AssignmentType = createPriceResource.AssignmentBase.AssignmentType,
                HourlyRate = createPriceResource.HourlyRate,
                UserId = createPriceResource.Id
            });
        }
    }
}
