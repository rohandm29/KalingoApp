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
            try
            {
                var captcha = await _apiClient.GetCaptcha(new CaptchaRequest(4, 2));

                return captcha;
            }
            catch (System.Exception)
            {
                return new CaptchaResponse(0, "");
            }
        }

        public async Task<CaptchaAnswerResponse> CaptchaSubmit(CaptchaAnswerRequest captchaAnswer)
        {
            try
            {
                var result = await _apiClient.SubmitCaptcha(captchaAnswer);

                return result;
            }
            catch (System.Exception)
            {
                return new CaptchaAnswerResponse(0, CaptchaCodes.NotFound);
            }
        }
    }
}