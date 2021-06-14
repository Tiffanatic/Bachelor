using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using RapidTime.Frontend.Models;

namespace RapidTime.Frontend.ProtoClients
{
    public class CompanyTypeGrpcClient
    {
        public CompanyTypeGrpcClient()
        {
            
        }

        public async Task<CompanyTypeResponse> CreateCompanyType(CreatyCompanyTypeResource createCompanyTypeResource)
        {
            var client = GetClient();
            CreateCompanyTypeRequest request = new CreateCompanyTypeRequest()
            {
                CompanyTypeName = createCompanyTypeResource.Name
            };
            var resp = client.Result.CreateCompanyType(request);

            return resp;
        }

        public async Task<CompanyTypeResponse> GetCompanyType(int CompanyTypeId)
        {
            var client = await GetClient();
            var res = client.GetCompanyType(new() {
                Id = CompanyTypeId
            });

            return res;
        }


        public async Task<List<CompanyTypeResponse>> GetAllCompanyTypes()
        {
            var client = await GetClient();
            return client.MultiCompanyType(new Empty()).Response.ToList();
        }
        
        
        private async Task<CompanyType.CompanyTypeClient> GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new CompanyType.CompanyTypeClient(channel);
            return client;
        }
    }
}