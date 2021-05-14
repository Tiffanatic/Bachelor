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

        public CompanyTypeResponse CreateCompanyType(CreatyCompanyTypeResource creatyCompanyTypeResource)
        {
            var client = GetClient();
            CreateCompanyTypeRequest request = new CreateCompanyTypeRequest()
            {
                Input = new CompanyTypeBase()
                {
                    CompanyTypeName = creatyCompanyTypeResource.Name
                }
            };
            var resp = client.CreateCompanyType(request);

            return resp;
        }
        
        
        public List<CompanyTypeBase> GetAllCompanyTypes()
        {
            var client = GetClient();
            var response = client.MultiCompanyType(new Empty());
            return response.ResponseList.ToList();
        }


        private CompanyType.CompanyTypeClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new CompanyType.CompanyTypeClient(channel);
            return client;
        }

    }
}