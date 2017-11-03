using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Kalingo.Master;
using Kalingo.Api.Client.Services;

namespace Kalingo.Activities
{
    [Activity(Label = "Captcha", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class CaptchaActivity : BaseActivity
    {
        private CaptchaService _captchaService;

        private int captchaId;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Captcha);

            await Initialise();
            RegisterControls();
        }

        private async Task Initialise()
        {
        }

        public void GetImage(string stream)
        {
            var imageBytes = Convert.FromBase64String(stream);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);

            var captcha = FindViewById<ImageView>(Resource.Id.imgCaptcha);

            captcha.SetImageBitmap(bitmap);
        }

        private void RegisterControls()
        {
            var btnSubmit = FindViewById<Button>(Resource.Id.btnCaptchaSubmit);
            btnSubmit.Click += btnSubmit_OnClick;
        }

        private async void btnSubmit_OnClick(object sender, EventArgs eventArgs)
        {
            _captchaService = new CaptchaService();

            var image = await _captchaService.GetCaptcha();

            captchaId = image.Id;

            GetImage(image.Image);

            //var captchaAnswer = FindViewById<TextView>(Resource.Id.txtCaptcha);

            //var result = await _captchaService.CaptchaSubmit(new CaptchaAnswer(captchaId, captchaAnswer.Text, App.GameId));

            //if (result.Match)
            //{
            //    // allocate coins
            //}
            //else
            //{
            //    // captcha
            //}
        }
    }
}