using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using RapidTime.Frontend.Models;

namespace RapidTime.Frontend.ProtoClients
{
    public class CountryGrpcClient
    {
        public CountryGrpcClient()
        {
            
        }

        public async Task createCountry(CreateCountryResource countryResourceToCreate)
        {
            var client = await GetClient();
            var res = client.CreateCountry(new CreateCountryRequest()
            {
                CountryCode = countryResourceToCreate.CountryCode,
                CountryName = countryResourceToCreate.CountryName
            });
        }
        
        private async Task<Country.CountryClient> GetClient()
        {
            
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Country.CountryClient(channel);
            return client;
        }

        
        public async Task<List<CountryBase>> GetAllCountries()
        {
            var client = await GetClient();
            return client.GetAllCountries(new  Empty()).Response.ToList();
        }

        public async Task DeleteCountry(int id)
        {
            var client = await GetClient();
            client.DeleteCountryAsync(new DeleteCountryRequest() {Id = id});
        }

        public async Task<CountryBase> GetCountry(int id)
        {
            var client = await GetClient();

            return client.GetCountry(new GetCountryRequest() {Id = id}).Response;
        }

        public async Task<CountryResponse> UpdateCountry(CountryBase countryBase)
        {
            var client = await GetClient();
            UpdateCountryRequest response = new UpdateCountryRequest()
            {
                Request = countryBase
            };
            return await client.UpdateCountryAsync(response);
        }
    }
}