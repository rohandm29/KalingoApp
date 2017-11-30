using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.Captcha;
using Kalingo.Games.Contract.Entity.MinesBoom;
using Kalingo.Games.Contract.Entity.User;
using Kalingo.Games.Contract.Entity.Voucher;
using Newtonsoft.Json;

namespace Kalingo.Api.Client.Client
{
    public class KalingoApiClient
    {
        private readonly string _baseAddress;
        private static string _authHeader;
        public IEnumerable<string> Headers;

        public KalingoApiClient()
        {
            _baseAddress = "http://kalingoapi.cloudapp.net/";
            //_baseAddress = "http://10.0.3.2:9988/";
        }

        public async Task<T> GetResponse<T>(HttpRequestMessage message)
        {
            var client = new HttpClient();

            var response = await client.SendAsync(message);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                response.Headers.TryGetValues("Auth", out Headers);

                return JsonConvert.DeserializeObject<T>(result);
            }

            throw new Exception();
        }

        // USER
        public async Task<AddUserResponse> AddUser(NewUserRequest user)
        {
            var uri = new Uri(_baseAddress + "/users/Add");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
            };

            var response = await GetResponse<AddUserResponse>(message);

            return response;
        }

        public async Task<UserResponse> GetUser(UserArgs userArgs)
        {
            var uri = new Uri(_baseAddress + "users/Get");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(userArgs), Encoding.UTF8, "application/json")
            };

            var response = await GetResponse<UserResponse>(message);

            GetHeaders();

            return response;
        }

        private void GetHeaders()
        {
            _authHeader = Headers?.FirstOrDefault() ?? string.Empty;
        }

        public async Task UpdateUser(UpdateUserRequest updateUser)
        {
            var uri = new Uri(_baseAddress + "users/Update");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(updateUser), Encoding.UTF8, "application/json")
            };
            AddHeader(message, updateUser.UserId);

            await GetResponse<UserResponse>(message);
        }

        public async Task<int> GetUserLimit(string userId)
        {
            var uri = new Uri(_baseAddress + "users/GetLimit");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            AddHeader(message, userId.ToString());

            var gameId = await GetResponse<int>(message);

            return gameId;
        }

        // MINESBOOM
        public async Task<int> CreateMinesBoom(string userId)
        {
            //var uri = new Uri("http://10.0.3.2:9988/game/join?gameTypeId=2&userId=1");
            var uri = new Uri(_baseAddress + $"/games/join?gameTypeId={App.MinesBoomId}&userId={userId}");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new GameArgs(0, userId, App.MinesBoomId)), Encoding.UTF8, "application/json")
            };

            AddHeader(message, userId.ToString());

            var gameId = await GetResponse<int>(message);

            return gameId;
        }

        public async Task<MinesboomSelectionResponse> SubmitMinesBoom(MinesboomSelectionRequest mbArgs)
        {
            var uri = new Uri(_baseAddress + "/games/submit/MinesBoomSession");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mbArgs), Encoding.UTF8, "application/json")
            };
            AddHeader(message, mbArgs.UserId.ToString());

            // new ObjectContent(typeof(MinesBoomArgs), (MinesBoomArgs)gameArgs, new JsonMediaTypeFormatter());

            var gameResult = await GetResponse<MinesboomSelectionResponse>(message);

            return gameResult;
        }

        public async Task TerminateMinesBoom(GameArgs gameArgs)
        {
            var uri = new Uri(_baseAddress + "/games/terminate");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(gameArgs), Encoding.UTF8, "application/json")
            };
            AddHeader(message, gameArgs.UserId.ToString());

            await GetResponse<MinesboomSelectionResponse>(message);
        }

        // CAPTCHA
        public async Task<CaptchaResponse> GetCaptcha(CaptchaRequest captchaArgs)
        {
            var uri = new Uri(_baseAddress + "captcha/Get");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(captchaArgs), Encoding.UTF8, "application/json")
            };
            AddHeader(message,captchaArgs.UserId.ToString());

            return await GetResponse<CaptchaResponse>(message);
        }

        public async Task<CaptchaAnswerResponse> SubmitCaptcha(CaptchaAnswerRequest captchaAnswer)
        {
            var uri = new Uri(_baseAddress + "captcha/Submit");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(captchaAnswer), Encoding.UTF8, "application/json")
            };
            AddHeader(message);

            return await GetResponse<CaptchaAnswerResponse>(message);
        }

        // VOUCHER
        public async Task<IEnumerable<VoucherResponse>> GetVouchers(int countryId)
        {
            var uri = new Uri(_baseAddress + $"voucher/Get?countryId={countryId}");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            AddHeader(message);

            return await GetResponse<IEnumerable<VoucherResponse>>(message);
        }

        public async Task<VoucherClaimResponse> SubmitClaim(VoucherClaimRequest claim)
        {
            var uri = new Uri(_baseAddress + "voucher/claim");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(claim), Encoding.UTF8, "application/json")
            };
            AddHeader(message, claim.UserId.ToString());

            return await GetResponse<VoucherClaimResponse>(message);
        }

        // COUNTRY
        public async Task<IEnumerable<CountryResponse>> GetCountries()
        {
            var uri = new Uri(_baseAddress + "country/Get");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);
            AddHeader(message);

            return await GetResponse<IEnumerable<CountryResponse>>(message);
        }

        private void AddHeader(HttpRequestMessage message, string userId = null)
        {
            message.Headers.Add("Auth", new[] { _authHeader });
            message.Headers.Add("UserId", new[] { userId });
        }
    }
}