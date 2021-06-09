using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using RapidTime.Frontend.Models;

namespace RapidTime.Frontend.ProtoClients
{
    public class CityGrpcClient
    {
        public CityGrpcClient()
        {
            
        }
        
         private City.CityClient GetClient()
         {
             var channel = GrpcChannel.ForAddress("https://localhost:5001");
             var client = new City.CityClient(channel);
             return client;
         }

         public void CreateCity(CreateCityResource cityResource)
         {
             var client = GetClient();
             client.CreateCity(new CreateCityRequest()
             {
                CityName = cityResource.CityName,
                PostalCode = cityResource.PostalCode,
                Country = cityResource.CountryName
                
             });
         }

         public List<CityResponse> GetAllCities()
         {
             var client = GetClient();
             var cities = client.MultiCity(new GetCityRequest());
             
             List<CityResponse> cityResponses = new List<CityResponse>();
             foreach (var city in cities.Response)
             {
                 cityResponses.Add(new CityResponse()
                 {
                     CityName = city.CityName,
                     PostalCode = city.PostalCode,
                     Country = city.Country,
                     Id = city.Id
                 });
             }

             return cityResponses;
         }


         public void DeleteCity(int cityId)
         {
             var client = GetClient();
             client.DeleteCity(new DeleteCityRequest()
             {
                 Id = cityId
             });
         }
    }
    
}
    
   