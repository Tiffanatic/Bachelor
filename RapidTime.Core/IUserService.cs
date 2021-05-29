using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RapidTime.Core.Models.Auth;

namespace RapidTime.Core
{
    public interface IUserService
    {
        Task<User> CreateUser(User input);
        Task DeleteUser(string id);
        Task<User> UpdateUser(User input);
        Task<User> GetUser(string id);
        IEnumerable<User> GetAllUsers();

        // Ingen "usage" pga. grpc implementering der kræver at .ToTimeStamp() er returneret.
        Task<DateTime> GetUserDeleteDate(string id);
        void SetUserDeleteDate(string id, DateTime deleteDate);
    }
}