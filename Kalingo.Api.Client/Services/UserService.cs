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

        public async Task<UserResponse> RegisterUser(string userName, string password, string email, int countryId, string deviceId, string version)
        {
            try
            {
                var user = new NewUserRequest(userName, Encryption.ComputeHash(password), email, countryId, deviceId, version);
                var userResponse = await _apiClient.AddUser(user);

                App.SaveSessionState(userResponse);

                return userResponse;
            }
            catch (System.Exception)
            {
                return default(UserResponse);
            }
        }

        public async Task<bool> UpdateUser(string email)
        {
            try
            {
                var updateUser = new UpdateUserRequest(App.UserId, email, 0);

                var result = await _apiClient.UpdateUser(updateUser);

                return result;
            }
            catch (System.Exception)
            {
                return false;
                // ignored
            }
        }

        public async Task<UserResponse> GetUser(string userName, string password)
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

        public async Task<UserResponse> GetFbUser(string userName)
        {
            try
            {
                var user = await _apiClient.GetFbUser(userName);

                return user;
            }
            catch (System.Exception)
            {
                return new UserResponse(0);
            }
        }

        public async Task<UserResponse> AddFbUser(string userName, string token, string emailAddress, int countryId, string deviceId, string version)
        {
            try
            {
                var fbUser = new FbUser(userName, token, emailAddress, countryId, deviceId, version);

                var user = await _apiClient.AddFbUser(fbUser);

                return user;
            }
            catch (System.Exception)
            {
                return new UserResponse(0);
            }
        }
        public async Task<int> GetUserLimit()
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

            App.SaveSessionState(user);

            return user;
        }
    }
}