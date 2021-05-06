using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using Microsoft.Extensions.Logging;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;
using RapidTime.Services;

namespace RapidTime.Api.GRPCServices
{
    public class CityGrpcService : City.CityBase
    {
        private ICityService _cityService;
        private ICountryService _countryService;
        private readonly ILogger<CityGrpcService> _logger;
        
        public CityGrpcService(ICityService cityService, ILogger<CityGrpcService> logger, ICountryService countryService)
        {
            _cityService = cityService;
            _logger = logger;
            _countryService = countryService;
        }

        public override Task<CityResponse> GetCity(GetCityRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Get City was called");
            var city = _cityService.FindById(request.Id);
            return Task.FromResult(new CityResponse()
            {
                Citybase =
                {
                    Id = city.Id,
                    CityName = city.CityName,
                    PostalCode = city.PostalCode
                }
            });
        }

        public override Task<MultiCityResponse> MultiCity(GetCityRequest request, ServerCallContext context)
        {
            _logger.LogInformation("MultiCity called");
            var cities = _cityService.GetAllCities();
            var response = new MultiCityResponse()
            {   
                Response = { }
            };
            var cityBases = cityEntitiesToCityResponse(cities);
            response.Response.AddRange(cityBases);
            return Task.FromResult(response);
        }

        public override Task<Empty> CreateCity(CreateCityRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Create City Called");
            var country = _countryService.GetCountryByNameOrCountryCode(request.Country);
            CityEntity cityEntity = new()
            {
                PostalCode = request.PostalCode,
                CityName = request.CityName,
                CountryEntity = country[0],
                CountryId = country[0].Id
            };
            _cityService.Insert(cityEntity);
            
            return Task.FromResult(new Empty());
        }
        
        
        public override async Task FindCity(
            IAsyncStreamReader<FindCityRequest> requestStream, 
            IServerStreamWriter<MultiCityResponse> responseStream, 
            ServerCallContext context)
        {
            var clientToServerTask = ClientToServerFindCityHandlingAsync(requestStream, context);

            var ServerToClientTask = ServerToClientFindCityHandlingAsync(responseStream, context);

            await Task.WhenAll(clientToServerTask, ServerToClientTask);
        }

        private CityEntity[] cities = System.Array.Empty<CityEntity>();
        private async Task ClientToServerFindCityHandlingAsync(IAsyncStreamReader<FindCityRequest> requestStream,
            ServerCallContext context)
        {
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                cities = _cityService.FindCityByNameOrPostalCode(requestStream.Current.Input);
            }
        }

        private async Task ServerToClientFindCityHandlingAsync(IServerStreamWriter<MultiCityResponse> responseStream,
            ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested && cities.Length > 0)
            {
                var response = new List<MultiCityResponse>();
                var cityBases = cityEntitiesToCityResponse(cities);
                var multiCity = new MultiCityResponse();
                multiCity.Response.AddRange(cityBases);
                await responseStream.WriteAsync(multiCity);
            }

            await Task.Delay(500);
        }

        //mapping from _cityService.GetAllCities to City.Proto.CityResponse for the FindAllCities Unary Call
        //Can also map 1 item from city to CityResponse 

        // private CityResponse CityResponseFromEntity(CityEntity cityEntity)
        // {
        //     return new CityResponse()
        //     {
        //         Citybase =
        //         {
        //             CityName = cityEntity.CityName,
        //             PostalCode = cityEntity.PostalCode,
        //             Id = cityEntity.Id
        //         }
        //     };
        // }
        
        private IEnumerable<CityResponse> cityEntitiesToCityResponse(IEnumerable<CityEntity> cities)
        {
            List<CityResponse> cityResponses = new List<CityResponse>();
            foreach (var city in cities)
            {
                cityResponses.Add(new CityResponse()
                {
                    Citybase = new CityBase()
                    {
                        CityName = city.CityName,
                        Id = city.Id,
                        PostalCode = city.PostalCode
                    }
                });
            }
            return cityResponses;
        }
    }
}