using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;
using RapidTime.Services;

namespace RapidTime.Api.GRPCServices
{
    public class CompanyTypeGrpcService : CompanyType.CompanyTypeBase
    {
        private ILogger<CompanyTypeGrpcService> _logger;
        private ICompanyTypeService _companyTypeService;

        public CompanyTypeGrpcService(ICompanyTypeService companyTypeService, ILogger<CompanyTypeGrpcService> logger)
        {
            _companyTypeService = companyTypeService;
            _logger = logger;
        }

        public override Task<CompanyTypeResponse> GetCompanyType(GetCompanyTypeRequest request, ServerCallContext context)
        {
            var companyTypeEntity = _companyTypeService.findById(request.Id);
            return Task.FromResult(new CompanyTypeResponse()
            {
                CompanyTypeName = companyTypeEntity.CompanyTypeName,
                Id = companyTypeEntity.Id
            });
        }

        public override Task<CompanyTypeResponse> CreateCompanyType(CreateCompanyTypeRequest request, ServerCallContext context)
        {
            CompanyTypeEntity companyTypeEntity = new()
            {
                CompanyTypeName = request.CompanyTypeName
            };

            var id = _companyTypeService.Insert(companyTypeEntity);
            var companyTypeToReturn = _companyTypeService.findById(id);
            return Task.FromResult(new CompanyTypeResponse()
            {
                CompanyTypeName = companyTypeToReturn.CompanyTypeName,
                Id = companyTypeToReturn.Id
            });
        }

        public override Task<Empty> DeleteCompanyType(DeleteCompanyTypeRequest request, ServerCallContext context)
        {
            _companyTypeService.Delete(request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<MultiCompanyTypeResponse> MultiCompanyType(Empty request, ServerCallContext context)
        {
            var companyTypeEntities = _companyTypeService.GetAll();
            MultiCompanyTypeResponse response = new();
            foreach (var companyTypeEntity in companyTypeEntities)
            {
                response.Response.Add(new CompanyTypeResponse()
                {
                    CompanyTypeName = companyTypeEntity.CompanyTypeName,
                    Id = companyTypeEntity.Id
                });
            }
            return Task.FromResult(response);
        }
    }
}