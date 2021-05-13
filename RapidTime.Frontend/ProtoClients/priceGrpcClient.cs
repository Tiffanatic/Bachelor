using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace RapidTime.Frontend.ProtoClients
{
    public class priceGrpcClient
    {

       
        public priceGrpcClient()
        {
            
        }

        private async Task<Price.PriceClient> GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Price.PriceClient(channel);
            return client;
        }
    }
}
