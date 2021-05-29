using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

namespace RapidTime.Frontend.ProtoClients
{
    public class UserGrpcClient
    {

        public UserGrpcClient()
        {
            
        }
        
        

        private User.UserClient GetClient()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new User.UserClient(channel);
            return client;
        }

        public List<UserResponse> GetAllUsers()
        {
            var client = GetClient();
            var users = client.GetAllUsers(new Empty());

            return users.MultiUserResponse_.ToList();
        }

        public UserResponse GetUser(string id)
        {
            var client = GetClient();
            var user = client.GetUser(new GetUserRequest()
            {
                Id = id
            });
            
            return user;
        }

        public void CreateUser(UserBase user)
        {
            var client = GetClient();
            var userCreated = client.CreateUser(new CreateUserRequest()
            {
                Request = user
            });
        }

        public string GetDeleteDate(string id)
        {
            var client = GetClient();
            return client.GetUserDeleteDate(new GetUserRequest()
            {
                Id = id
            }).ToString();
        }

        public UserResponse SetDeleteDate(DateTime dateTime, string id)
        {
            var client = GetClient();
            var resp = client.SetUserDeleteDate(new SetUserDeleteDateRequest()
            {
                DeleteDate = Timestamp.FromDateTime(dateTime),
                Id = id
            });

            return resp;
        }
    }
}