using System.Collections.Generic;
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
                UserBaseResponse = new UserBase()
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
                UserBaseResponse = new UserBase()
                {
                    FirstName = userCreated.Firstname,
                    LastName = userCreated.Lastname,
                    Email = userCreated.Email,
                    GdprDeleted = userCreated.GdprDeleted,
                    PhoneNumber = userCreated.PhoneNumber,
                },
                UserId = userCreated.Id.ToString()
            };
        
            return response;
        }

        public override async Task<Empty> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            await _userService.DeleteUser(request.Id);
            return new Empty();
        }

        public override async Task<UserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            var userFound = await _userService.GetUser(request.Id);
            
            var userEntityToBeUpdated = new Core.Models.Auth.User()
            {
                Firstname = request.FirstName,
                Lastname = request.LastName,
                GdprDeleted = request.GdprDeleted,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email
            };
        
            var userCreated = await _userService.UpdateUser(userEntityToBeUpdated);
        
            UserResponse response = new UserResponse()
            {
                UserBaseResponse = new UserBase()
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

        public override Task<MultiUserResponse> GetAllUsers(Empty request, ServerCallContext context)
        {
            var users = _userService.GetAllUsers();
        
            var responseObject = new MultiUserResponse() {MultiUserResponse_ = {new List<UserResponse>()}};
            
            foreach (RapidTime.Core.Models.Auth.User user in users)
            {
                var userToAdd = new UserBase()
                {
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Email = user.Email,
                    GdprDeleted = user.GdprDeleted,
                    PhoneNumber = user.PhoneNumber
                };
                var userResponseToAdd = new UserResponse()
                {
                    UserBaseResponse = new UserBase(userToAdd),
                    UserId = user.Id.ToString()
                };
                
                responseObject.MultiUserResponse_.Add(userResponseToAdd);
            }
            
            return Task.FromResult(responseObject);
        }

        public override async Task<DeleteDateResponse> GetUserDeleteDate(GetUserRequest request, ServerCallContext context)
        {
            var user = await _userService.GetUser(request.Id);
            var userDeleteDate = user.DeleteDate.ToUniversalTime().ToTimestamp();
            
            var response = new DeleteDateResponse()
            {
                DeleteDate = userDeleteDate
            };

            return response;
        }

        public override async Task<UserResponse> SetUserDeleteDate(SetUserDeleteDateRequest request, ServerCallContext context)
        {
            var user = await _userService.GetUser(request.Id);
            var userDeleteDate = user.DeleteDate.ToUniversalTime().ToTimestamp();
            
            _userService.SetUserDeleteDate(user.Id.ToString(), request.DeleteDate.ToDateTime());
        
            user = await _userService.GetUser(request.Id);
        
            var response = new UserResponse()
            {
                UserBaseResponse = new UserBase()
                {
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Email = user.Email,
                    GdprDeleted = user.GdprDeleted,
                    PhoneNumber = user.PhoneNumber
                }
            };
            
            return response;
        }
    }
}