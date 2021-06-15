using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace RapidTime.Frontend.ProtoClients
{
    public class TimeRegistrationGrpcClient
    {
        private async Task<TimeRegistration.TimeRegistrationClient> GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new TimeRegistration.TimeRegistrationClient(channel);
            return client;
        }

        public async void RegisterTime(CreateTimeRegistrationRequest request)
        {
            var client = await GetClient();
            var res = client.CreateTimeRegistration(request);
            if (res.Success != true)
            {
                throw new Exception();
            }
        }
    }
}