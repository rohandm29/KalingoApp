using System;
using System.Collections.Generic;
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
                return JsonConvert.DeserializeObject<T>(result);
            }

            throw new Exception();
        }

        // USER
        public async Task<int> AddUser(NewUserRequest user)
        {
            var uri = new Uri(_baseAddress + "/users/Add");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
            };

            var response = await GetResponse<int>(message);

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

            return response;
        }

        public async Task UpdateUser(UpdateUserRequest updateUser)
        {
            var uri = new Uri(_baseAddress + "users/Update");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(updateUser), Encoding.UTF8, "application/json")
            };

            await GetResponse<UserResponse>(message);
        }

        // MINESBOOM
        public async Task<int> CreateMinesBoom(int userId)
        {
            //var uri = new Uri("http://10.0.3.2:9988/game/join?gameTypeId=2&userId=1");
            var uri = new Uri(_baseAddress + $"/games/join?gameTypeId={App.MinesBoomId}&userId={userId}");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);

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

            return await GetResponse<CaptchaResponse>(message);
        }

        public async Task<CaptchaAnswerResponse> SubmitCaptcha(CaptchaAnswerRequest captchaAnswer)
        {
            var uri = new Uri(_baseAddress + "captcha/Submit");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(captchaAnswer), Encoding.UTF8,
                    "application/json")
            };

            return await GetResponse<CaptchaAnswerResponse>(message);
        }

        // VOUCHER
        public async Task<IEnumerable<VoucherResponse>> GetVouchers(int countryId)
        {
            var uri = new Uri(_baseAddress + $"voucher/Get?countryId={countryId}");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);

            return await GetResponse<IEnumerable<VoucherResponse>>(message);
        }

        public async Task<VoucherClaimResponse> SubmitClaim(VoucherClaimRequest claim)
        {
            var uri = new Uri(_baseAddress + "voucher/claim");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(claim), Encoding.UTF8, "application/json")
            };

            return await GetResponse<VoucherClaimResponse>(message);
        }

        // COUNTRY
        public async Task<IEnumerable<CountryResponse>> GetCountries()
        {
            var uri = new Uri(_baseAddress + "country/Get");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);

            return await GetResponse<IEnumerable<CountryResponse>>(message);
        }
    }
}