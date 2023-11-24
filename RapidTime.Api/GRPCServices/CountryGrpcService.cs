using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models.Address;

namespace RapidTime.Api.GRPCServices
{
    public class CountryGrpcService : Country.CountryBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryGrpcService> _logger;

        public CountryGrpcService(ICountryService countryService, ILogger<CountryGrpcService> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        public override Task<Empty> CreateCountry(CreateCountryRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Create Country Called");
            _countryService.Insert(new CountryEntity()
            {
                CountryName = request.CountryName,
                CountryCode = request.CountryCode
            });
            
            return Task.FromResult(new Empty());
        }

        public override Task<Empty> DeleteCountry(DeleteCountryRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Delete Country called on Id: {Id}", request.Id);
            _countryService.DeleteCountry(request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<CountryResponse> GetCountry(GetCountryRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Get Country Called with Id: {Id}", request.Id);
            var countryEntity = _countryService.FindById(request.Id);
            return Task.FromResult(new CountryResponse()
            {
                CountryCode = countryEntity.CountryCode,
                CountryName = countryEntity.CountryName,
                Id = countryEntity.Id
            });
        }

        public override Task<CountryResponse> UpdateCountry(UpdateCountryRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Update Country called with Id: {Id}", request.Id);
            var countryEntity = _countryService.FindById(request.Id);
            countryEntity.CountryCode = request.CountryCode;
            countryEntity.CountryName = request.CountryName;
            _countryService.Update(countryEntity);
            return Task.FromResult(new CountryResponse()
            {
                Id = countryEntity.Id,
                CountryCode = countryEntity.CountryCode,
                CountryName = countryEntity.CountryName
            });
        }

        public override Task<MultiCountryResponse> GetAllCountries(Empty request, ServerCallContext context)
        {
            var countries = _countryService.GetAllCountries();

            MultiCountryResponse response = new();

            List<CountryResponse> countryResponses = new();

            foreach (var country in countries)
            {
                countryResponses.Add(new CountryResponse
                {
                    Id = country.Id,
                    CountryCode = country.CountryCode,
                    CountryName = country.CountryName
                });
            }
            
            response.Response.AddRange(countryResponses);

            return Task.FromResult(response);
        }
    }
}