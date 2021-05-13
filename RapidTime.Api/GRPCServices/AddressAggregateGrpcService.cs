using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models.Address;
using RapidTime.Services;

namespace RapidTime.Api.GRPCServices
{
    public class AddressAggregateGrpcService : AddressAggregate.AddressAggregateBase
    {
        private IAddressAggregateService _addressAggregateService;
        private readonly ILogger<AddressAggregateGrpcService> _logger;
        private ICityService _cityService;
        private ICountryService _countryService;
    
        public AddressAggregateGrpcService(IAddressAggregateService addressAggregateService,
            ILogger<AddressAggregateGrpcService> logger, ICityService cityService)
        {
            _addressAggregateService = addressAggregateService;
            _logger = logger;
            _cityService = cityService;
        }
    
        public override Task<AddressAggregateResponse> CreateAddressAggregate(CreateAddressAggregateRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Create Address Called with street {street}, for customerId {customer}",
                request.Request.Street, request.CustomerId);
            var addressToInsert = new AddressAggregateEntity()
            {
                Street = request.Request.Street,
                CityId = _cityService.FindCityByNameOrPostalCode(request.Request.City.CityName).First().Id,
                CountryId = _countryService.GetCountryByNameOrCountryCode(request.Request.Country.CountryName).First().Id,
                CustomerId = request.CustomerId
            };
            var id = _addressAggregateService.Insert(addressToInsert);
            var addressAggregateEntityToReturn = _addressAggregateService.FindById(id);
            var addressToReturn = AddressAggregateResponse(addressAggregateEntityToReturn);
            return Task.FromResult(addressToReturn);
        }
    
        private static AddressAggregateResponse AddressAggregateResponse(
            AddressAggregateEntity addressAggregateEntityToReturn)
        {
            AddressAggregateResponse addressToReturn = new()
            {
                Response =
                {
                    Street = addressAggregateEntityToReturn.Street,
                    City =
                    {
                        CityName = addressAggregateEntityToReturn.CityEntity.CityName,
                        PostalCode = addressAggregateEntityToReturn.CityEntity.PostalCode
                    },
                    Country =
                    {
                        CountryCode = addressAggregateEntityToReturn.CountryEntity.CountryCode,
                        CountryName = addressAggregateEntityToReturn.CountryEntity.CountryName
                    }
                }
            };
            return addressToReturn;
        }
    
        public override Task<Empty> DeleteAddressAggregate(DeleteAddressAggregateRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Delete Address called on Id: {Id}", request.Id);
            _addressAggregateService.Delete(request.Id);
    
            return Task.FromResult(new Empty());
        }
    
        public override Task<AddressAggregateResponse> GetAddressAggregate(GetAddressAggregateRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Get Address called with Id: {Id}", request.Id);
            var address = _addressAggregateService.FindById(request.Id);
    
            AddressAggregateResponse addressAggregateResponse = AddressAggregateResponse(address);
    
            return Task.FromResult(addressAggregateResponse);
        }
    
        public override Task<Empty> UpdateAddressAggregate(UpdateAddressAggregateRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Update Address Called on Id: {Id}", request.Id);
            var addressToUpdate = _addressAggregateService.FindById(request.Id);
            addressToUpdate.Street = request.Response.Street;
            addressToUpdate.CityId = request.Response.City.Id;
            addressToUpdate.CountryId = request.Response.Country.Id;
            _addressAggregateService.Update(addressToUpdate);
        
            return Task.FromResult(new Empty());
        
        }

        public override Task<MultiAddressAggregateResponse> GetAddressByCustomerId(GetAddressAggregateRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Get Addresses for customer with Id: {Id}", request.Id);
            var rawAddress = _addressAggregateService.GetAll();
            var addressToReturn = rawAddress.Where(x => x.CustomerId == request.Id);

            MultiAddressAggregateResponse multiAddressAggregateResponse = new MultiAddressAggregateResponse();

            foreach (var address in addressToReturn)
            {
                multiAddressAggregateResponse.Response.Add(new AddressAggregateBase()
                {
                    Street = address.Street,
                    City = new CityBase()
                    {
                        CityName = address.CityEntity.CityName,
                        Country = address.CityEntity.CityName,
                        PostalCode = address.CityEntity.PostalCode,
                        Id = address.CityId
                    },
                    Country = new CountryBase()
                    {
                        CountryCode = address.CountryEntity.CountryCode,
                        CountryName = address.CountryEntity.CountryName,
                        Id = address.CountryId
                    }
                });
            }

            return Task.FromResult(multiAddressAggregateResponse);
        }
    }
}