using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RapidTime.Core;

namespace RapidTime.Api.GRPCServices
{
    
    
    public class UserGrpcService : User.UserBase
    {
        private ILogger<UserGrpcService> _logger;
        private readonly IUserService _userService;

        public UserGrpcService(ILogger<UserGrpcService> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }


        public override Task<UserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var user = _userService.GetUser(request.Id.ToString());
            UserResponse response = new UserResponse()
            {
                Response = new UserBase()
                {
                    FirstName = user.Result.Firstname,
                    LastName = user.Result.Lastname,
                    Email = user.Result.Email,
                    GdprDeleted = user.Result.GdprDeleted,
                    PhoneNumber = user.Result.PhoneNumber
                }
            };
            
            return Task.FromResult(response);
        }

        public override async Task<UserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            var userEntityToBeCreated = new Core.Models.Auth.User()
            {
                Firstname = request.Request.FirstName,
                Lastname = request.Request.LastName,
                GdprDeleted = request.Request.GdprDeleted,
                Email = request.Request.Email,
                PhoneNumber = request.Request.PhoneNumber,
                UserName = request.Request.Email
            };

            var userCreated = await _userService.CreateUser(userEntityToBeCreated);

            UserResponse response = new UserResponse()
            {
                Response = new UserBase()
                {
                    FirstName = userCreated.Firstname,
                    LastName = userCreated.Lastname,
                    Email = userCreated.Email,
                    GdprDeleted = userCreated.GdprDeleted,
                    PhoneNumber = userCreated.PhoneNumber
                }
            };

            return response;


        }
        
        
    }
}