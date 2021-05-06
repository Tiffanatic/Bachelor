using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;
using Serilog;

namespace RapidTime.Api.GRPCServices
{
    public class CountryGrpcService : Country.CountryBase
    {
        private ICountryService _countryService;
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
                Response = new CountryBase()
                {
                    CountryCode = countryEntity.CountryCode,
                    CountryName = countryEntity.CountryName,
                    Id = countryEntity.Id
                }
            });
        }

        public override Task<CountryResponse> UpdateCountry(UpdateCountryRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Update Country called with Id: {Id}", request.Request.Id);
            var countryEntity = _countryService.FindById(request.Request.Id);
            countryEntity.CountryCode = request.Request.CountryCode;
            countryEntity.CountryName = request.Request.CountryName;
            _countryService.Update(countryEntity);
            return Task.FromResult(new CountryResponse()
            {
                Response = new CountryBase()
                {
                    Id = countryEntity.Id,
                    CountryCode = countryEntity.CountryCode,
                    CountryName = countryEntity.CountryName
                }
            });
        }
    }
}