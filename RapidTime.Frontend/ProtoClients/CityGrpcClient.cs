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

         public List<CityBase> GetAllCities()
         {
             var client = GetClient();
             var cities = client.MultiCity(new GetCityRequest());
             
             List<CityBase> cityBases = new List<CityBase>();
             foreach (var city in cities.Response)
             {
                 cityBases.Add(city.Citybase);
             }

             return cityBases;
         }


         public void DeleteCity(int citybaseId)
         {
             var client = GetClient();
             client.DeleteCity(new DeleteCityRequest()
             {
                 Id = citybaseId
             });
         }
    }
    
}
    
   