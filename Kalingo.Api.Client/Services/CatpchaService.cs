using System.Threading.Tasks;
using Kalingo.Api.Client.Client;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.Captcha;

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
                var captcha = await _apiClient.GetCaptcha(new CaptchaRequest(App.UserId, App.GameId));

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