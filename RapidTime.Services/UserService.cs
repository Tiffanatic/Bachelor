using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RapidTime.Core;
using RapidTime.Core.Models.Auth;

namespace RapidTime.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<User> CreateUser(User input)
        {
            try
            {
                _logger.LogInformation("CreateUser called with param: {@input}", input);
                User userFound = await _userManager.FindByIdAsync(input.Id.ToString());
                if (userFound != null) throw new Exception();

                await _userManager.CreateAsync(input);
                return await _userManager.FindByIdAsync(input.Id.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"DeleteUser called with param: {id}", id);
                User userFound = _userManager.FindByIdAsync(id.ToString()).Result;
                if (userFound == null) throw new Exception();

                _userManager.DeleteAsync(userFound);
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", "{StackTrace}", e.Message, e.StackTrace);
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<User> UpdateUser(User input)
        {
            try
            {
                _logger.LogInformation($"UpdateUser called with param: {input}", input);
                var userFound = _userManager.FindByIdAsync(input.Id.ToString());
                if (userFound == null) throw new Exception();

                await _userManager.CreateAsync(input);
                return await _userManager.FindByIdAsync(input.Id.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", "{StackTrace}", e.Message, e.StackTrace);
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<User> GetUser(string id)
        {
            try
            {
                _logger.LogInformation($"GetUser called with param: {id}", id);
                if (string.IsNullOrWhiteSpace(id))
                    throw new ArgumentNullException("id", "Unable to parse parameter as an integer for id");
                var user = await _userManager.FindByIdAsync(id);
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", "{StackTrace}", e.Message, e.StackTrace);
                throw new ArgumentException(e.Message);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                var response = _userManager.Users.AsEnumerable();
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", "{StackTrace}", e.Message, e.StackTrace);
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<System.DateTime> GetUserDeleteDate(string id)
        {
            var user = await GetUser(id);
            return user.DeleteDate;
        }
    }
}