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

        public async Task<UserResponse> AuthenticateUser(string username, string password)
        {
            var validUser = await IsUserValid(username, password);

            return validUser;
        }

        public async Task<int> RegisterUser(string userName, string password, string email, int countryId)
        {
            try
            {
                var user = new NewUserRequest(userName, Encryption.ComputeHash(password), email, countryId);
                var userId = await _apiClient.AddUser(user);

                App.UserId = userId;

                return userId;
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public async Task UpdateUser(string email, int countryId)
        {
            try
            {
                var updateUser = new UpdateUserRequest(App.UserId, email, countryId);

                await _apiClient.UpdateUser(updateUser);
            }
            catch (System.Exception)
            {
                // ignored
            }
        }

        private async Task<UserResponse> GetUser(string userName, string password)
        {
            try
            {
                var userArgs = new UserArgs(userName, password);

                var user = await _apiClient.GetUser(userArgs);

                return user;
            }
            catch (System.Exception)
            {
                return new UserResponse(0);
            }
        }
        
        public async Task<UserResponse> FbUserLogin(string userName, string token)
        {
            try
            {
                var fbUser = new FbUser(userName, token);

                var user = await _apiClient.AddFbUser(fbUser);

                return user;
            }
            catch (System.Exception)
            {
                return new UserResponse(0);
            }
        }
        public async Task<int> GetUserLimit(int userId)
        {
            try
            {
                return await _apiClient.GetUserLimit(App.UserId);
            }
            catch (System.Exception)
            {
                return 0;
                // ignored
            }
        }

        private async Task<UserResponse> IsUserValid(string username, string password)
        {
            var user = await GetUser(username, password);

            SaveSessionState(user);

            return user;
        }

        private void SaveSessionState(UserResponse user)
        {
            App.UserId = user.UserId;
            App.Gold = user.Gold;
            App.Silver = user.Silver;
            App.CountryId = user.CountryId;
            App.PromoUser = user.PromoId;
        }
    }
}