using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Api.GRPCServices
{
    public class PriceGrpcService : Price.PriceBase
    {
        private ILogger<PriceGrpcService> _logger;
        private IPriceService _priceService;

        public PriceGrpcService(IPriceService priceService, ILogger<PriceGrpcService> logger)
        {
            _priceService = priceService;
            _logger = logger;
        }

        public override Task<PriceResponse> CreatePrice(CreatePriceRequest request, ServerCallContext context)
        {
            _logger.LogInformation("CreatePrice Called {@request}", request);
            var price = new PriceEntity()
            {
                HourlyRate = request.HourlyRate,
                UserId = Guid.Parse(request.UserId),
                AssignmentTypeId = request.AssignmentType.Id
            };
            var priceId = _priceService.Insert(price);
            var insertedPrice = _priceService.GetById(priceId);

            return Task.FromResult(PriceEntityToPriceResponse(insertedPrice));
        }

        public override Task<MultiPriceResponse> UserPrices(GetUserPriceListRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Userprices called Id: {request}", request.Id);
            var prices = _priceService.GetAll();
            var UserPrices = prices.Where(x => x.UserId == Guid.Parse(request.Id));
            var multiPriceResponse = new MultiPriceResponse();
            foreach (var price in UserPrices)
            {
                multiPriceResponse.Response.Add(new PriceResponse()
                {
                    Id = price.Id,
                    HourlyRate = price.HourlyRate,
                    AssignmentType =
                    {
                        Id = price.AssignmentTypeId,
                        InvoiceAble = price.AssignmentTypeEntity.InvoiceAble,
                        Name = price.AssignmentTypeEntity.Name,
                        Number = price.AssignmentTypeEntity.Number
                    }
                });
                
            }
            return Task.FromResult(multiPriceResponse);
        }

        public override Task<PriceResponse> GetPrice(GetPriceRequest request, ServerCallContext context)
        {
            _logger.LogInformation("GetPrice Called on Id: {Id}", request.Id);
            var price = _priceService.GetById(request.Id);
            return Task.FromResult(PriceEntityToPriceResponse(price));
        }

        public override Task<Empty> DeletePrice(DeletePriceRequest request, ServerCallContext context)
        {
            _logger.LogInformation("DeletePriceRequest Called on Id: {Id}", request.Id);
            _priceService.Delete(request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<PriceResponse> UpdateHourlyRate(UpdatePriceRequest request, ServerCallContext context)
        {
            _logger.LogInformation("UpdateHourlyRate called on Id: {Id}", request.Id);
            var price = _priceService.GetById(request.Id);
        
            price.HourlyRate = request.HourlyRate;
            
            _priceService.Update(price);
        
            return Task.FromResult(PriceEntityToPriceResponse(price));
        }

        private PriceResponse PriceEntityToPriceResponse(PriceEntity priceEntity)
        {
            return new PriceResponse()
            {
                Id = priceEntity.Id,
                HourlyRate = priceEntity.HourlyRate,
                AssignmentType =
                {
                    Id = priceEntity.AssignmentTypeId,
                    InvoiceAble = priceEntity.AssignmentTypeEntity.InvoiceAble,
                    Name = priceEntity.AssignmentTypeEntity.Name,
                    Number = priceEntity.AssignmentTypeEntity.Name
                }
            };
        }
        
        
    }
}