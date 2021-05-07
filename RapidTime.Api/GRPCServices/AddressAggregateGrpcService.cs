using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core.Models.Address;
using RapidTime.Core.Services;
using RapidTime.Services;

namespace RapidTime.Api.GRPCServices
{
    public class AddressAggregateGrpcService : AddressAggregate.AddressAggregateBase
    {
        private IAddressAggregateService _addressAggregateService;
        private readonly ILogger<AddressAggregateGrpcService> _logger;

        public AddressAggregateGrpcService(IAddressAggregateService addressAggregateService,
            ILogger<AddressAggregateGrpcService> logger)
        {
            _addressAggregateService = addressAggregateService;
            _logger = logger;
        }

        public override Task<AddressAggregateResponse> CreateAddressAggregate(CreateAddressAggregateRequest request,
            ServerCallContext context)
        {
            _logger.LogInformation("Create Address Called with street {street}, for customerId {customer}",
                request.Request.Street, request.CustomerId);
            var addressToInsert = new AddressAggregateEntity()
            {
                Street = request.Request.Street,
                CityId = request.Request.City.Id,
                CountryId = request.Request.Country.Id,
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
    }
}