using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;

namespace RapidTime.Api.GRPCServices
{
    
    
    public class UserGrpcService : User.UserBase
    {
        private ILogger<UserGrpcService> _logger;
        private IUserService _userService;

        public UserGrpcService(ILogger<UserGrpcService> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        public override Task<UserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            return base.CreateUser(request, context);
        }
    }
}