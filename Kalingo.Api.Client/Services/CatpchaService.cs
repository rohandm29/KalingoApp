using System.Threading.Tasks;
using Android.App;
using Kalingo.Api.Client.Client;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.Captcha;
using Kalingo.Games.Contract.Entity.User;

namespace Kalingo.Api.Client.Services
{
    public class CaptchaService
    {
        private readonly KalingoApiClient _apiClient;

        public CaptchaService()
        {
            _apiClient = new KalingoApiClient();
        }

       public async Task<CaptchaResponse> GetCaptcha()
        {
            var captcha = await _apiClient.GetCaptcha(new CaptchaArgs(4,2));

            return captcha;
        }

        public async Task<CaptchaResult> CaptchaSubmit(CaptchaAnswer captchaAnswer)
        {
            var result = await _apiClient.SubmitCaptcha(captchaAnswer);

            return result;
        }
    }
}