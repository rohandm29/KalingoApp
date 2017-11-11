using System;
using System.Net.Http;
using System.Threading.Tasks;
using Kalingo.Api.Client.Client;
using Kalingo.Core;
using Kalingo.Core.Encryption;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.User;

namespace Kalingo.Api.Client.Services
{
    public class UserService
    {
        private readonly KalingoApiClient _apiClient;

        public UserService()
        {
            _apiClient = new KalingoApiClient();
        }

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            var validUser = await IsUserValid(username, password);

            return validUser;
        }

        private async Task<bool> IsUserValid(string username, string password)
        {
            var user = await GetUser(username, password);

            SaveSessionState(user);
            
            return user.UserId != 0;
        }

        private void SaveSessionState(UserResponse user)
        {
            App.UserId = user.UserId;
            App.Gold = user.Gold;
            App.Silver = user.Silver;
            App.CountryId = user.CountryId;
            App.PromoUser = user.PromoId;
        }

        public async Task<int> RegisterUser(string userName, string password, string email, string country)
        {
            var user = new NewUser(userName, Encryption.ComputeHash(password), email, country);
            var userId = await _apiClient.AddUser(user);

            App.UserId = userId;

            return userId;
        }

        public async Task<UserResponse> GetUser(string userName, string password)
        {
            var userArgs = new UserArgs(userName, password);

            var user = await _apiClient.GetUser(userArgs);

            return user;
        }

        public async Task UpdateUser(string email, string country)
        {
            var updateUser = new UpdateUser(App.UserId, email, country);

            await _apiClient.UpdateUser(updateUser);
        }
    }
}