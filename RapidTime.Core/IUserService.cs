using System.Threading.Tasks;
using RapidTime.Core.Models.Auth;

namespace RapidTime.Core
{
    public interface IUserService
    {
        Task<User> CreateUser(User input);
        void DeleteUser(int id);
        Task<User> UpdateUser(User input);
        Task<User> GetUser(int id);
    }
}