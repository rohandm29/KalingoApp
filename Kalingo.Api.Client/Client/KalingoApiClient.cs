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
            try
            {
                var client = new HttpClient();

                var response = await client.SendAsync(message);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }

                return default(T);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> CreateMinesBoom(int userId)
        {
            //var uri = new Uri("http://10.0.3.2:9988/game/join?gameTypeId=2&userId=1");
            var uri = new Uri(_baseAddress + $"/games/join?gameTypeId={App.MinesBoomId}&userId={userId}");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);

            var gameId = await GetResponse<int>(message);

            return gameId;
        }

        public async Task<MinesBoomGameResult> SubmitMinesBoom(MinesBoomArgs mbArgs)
        {
            var uri = new Uri(_baseAddress + "/games/submit/MinesBoomSession");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(mbArgs), Encoding.UTF8, "application/json")
            };

            // new ObjectContent(typeof(MinesBoomArgs), (MinesBoomArgs)gameArgs, new JsonMediaTypeFormatter());

            var gameResult = await GetResponse<MinesBoomGameResult>(message);

            return gameResult;
        }

        public async Task TerminateMinesBoom(GameArgs gameArgs)
        {
            var uri = new Uri(_baseAddress + "/games/terminate");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(gameArgs), Encoding.UTF8, "application/json")
            };

            await GetResponse<MinesBoomGameResult>(message);
        }

        public async Task<int> AddUser(NewUser user)
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

        public async Task UpdateUser(UpdateUser updateUser)
        {
            var uri = new Uri(_baseAddress + "users/Update");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(updateUser), Encoding.UTF8, "application/json")
            };

            await GetResponse<UserResponse>(message);
        }

        public async Task<CaptchaResponse> GetCaptcha(CaptchaArgs captchaArgs)
        {
            var uri = new Uri(_baseAddress + "captcha/Get");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(captchaArgs), Encoding.UTF8, "application/json")
            };

            return await GetResponse<CaptchaResponse>(message);
        }

        public async Task<CaptchaResult> SubmitCaptcha(CaptchaAnswer captchaAnswer)
        {
            var uri = new Uri(_baseAddress + "captcha/Submit");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(captchaAnswer), Encoding.UTF8,
                    "application/json")
            };

            return await GetResponse<CaptchaResult>(message);
        }

        public async Task<IEnumerable<Voucher>> GetVouchers(int countryId)
        {
            var uri = new Uri(_baseAddress + $"voucher/Get?countryId={countryId}");

            var message = new HttpRequestMessage(HttpMethod.Get, uri);

            return await GetResponse<IEnumerable<Voucher>>(message);
        }

        public async Task<VoucherClaimResponse> SubmitClaim(VoucherClaim claim)
        {
            var uri = new Uri(_baseAddress + "voucher/claim");

            var message = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(claim), Encoding.UTF8,"application/json")
            };

            return await GetResponse<VoucherClaimResponse>(message);
        }
    }
}