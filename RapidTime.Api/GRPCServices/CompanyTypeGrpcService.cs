using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models;

namespace RapidTime.Api.GRPCServices
{
    public class CompanyTypeGrpcService(ICompanyTypeService companyTypeService, ILogger<CompanyTypeGrpcService> logger)
        : CompanyType.CompanyTypeBase
    {
        public override Task<CompanyTypeResponse> GetCompanyType(GetCompanyTypeRequest request, ServerCallContext context)
        {
            var companyTypeEntity = companyTypeService.FindById(request.Id);
            logger.LogInformation("Creating city response by {CompanyTypeEntityCompanyTypeName}", companyTypeEntity.CompanyTypeName);
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

            var id = companyTypeService.Insert(companyTypeEntity);
            var companyTypeToReturn = companyTypeService.FindById(id);
            return Task.FromResult(new CompanyTypeResponse()
            {
                CompanyTypeName = companyTypeToReturn.CompanyTypeName,
                Id = companyTypeToReturn.Id
            });
        }

        public override Task<Empty> DeleteCompanyType(DeleteCompanyTypeRequest request, ServerCallContext context)
        {
            companyTypeService.Delete(request.Id);
            return Task.FromResult(new Empty());
        }

        public override Task<MultiCompanyTypeResponse> MultiCompanyType(Empty request, ServerCallContext context)
        {
            var companyTypeEntities = companyTypeService.GetAll();
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