using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core.Models;
using RapidTime.Core.Services;
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
                Response =
                {
                    CompanyTypeName = companyTypeEntity.CompanyTypeName,
                    Id = companyTypeEntity.Id
                }
            });
        }

        public override Task<CompanyTypeResponse> CreateCompanyType(CreateCompanyTypeRequest request, ServerCallContext context)
        {
            CompanyTypeEntity companyTypeEntity = new()
            {
                CompanyTypeName = request.Input.CompanyTypeName
            };

            var id = _companyTypeService.Insert(companyTypeEntity);
            var companyTypeToReturn = _companyTypeService.findById(id);
            return Task.FromResult(new CompanyTypeResponse()
            {
                Response =
                {
                    CompanyTypeName = companyTypeToReturn.CompanyTypeName,
                    Id = companyTypeToReturn.Id
                }
            });
        }

        public override Task<Empty> DeleteCompanyType(DeleteCompanyTypeRequest request, ServerCallContext context)
        {
            _companyTypeService.Delete(request.Id);
            return Task.FromResult(new Empty());
        }
    }
}