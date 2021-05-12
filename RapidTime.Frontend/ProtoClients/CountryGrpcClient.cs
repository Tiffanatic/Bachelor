using System.Threading.Tasks;
using Grpc.Net.Client;
using RapidTime.Frontend.Models;

namespace RapidTime.Frontend.ProtoClients
{
    public class CountryGrpcClient
    {
        public CountryGrpcClient()
        {
            
        }

        public async Task createCountry(CreateCountry CountryToCreate)
        {
            var client = await GetClient();
            var res = client.CreateCountry(new CreateCountryRequest()
            {
                CountryCode = CountryToCreate.CountryCode,
                CountryName = CountryToCreate.CountryName
            });
        }
        
        private async Task<Country.CountryClient> GetClient()
        {
            
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Country.CountryClient(channel);
            return client;
        }
    }
}