using System.Collections.Generic;
using System.Linq;
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

        public CompanyTypeResponse CreateCompanyType(CreatyCompanyTypeResource createCompanyTypeResource)
        {
            var client = GetClient();
            CreateCompanyTypeRequest request = new CreateCompanyTypeRequest()
            {
                CompanyTypeName = createCompanyTypeResource.Name
            };
            var resp = client.CreateCompanyType(request);

            return resp;
        }

        public CompanyTypeResponse GetCompanyType(int CompanyTypeId)
        {
            var client = GetClient();
            var res = client.GetCompanyType(new() {
                Id = CompanyTypeId
            });

            return res;
        }


        public List<CompanyTypeResponse> GetAllCompanyTypes()
        {
            var client = GetClient();
            var response = client.MultiCompanyType(new Empty());
            return response.Response.ToList();
        }


        private CompanyType.CompanyTypeClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new CompanyType.CompanyTypeClient(channel);
            return client;
        }

    }
}