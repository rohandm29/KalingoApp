using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Kalingo.Api.Client.Services;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.Captcha;

namespace Kalingo.Activities
{
    [Activity(Label = "Captcha", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class CaptchaActivity : Activity
    {
        private CaptchaService _captchaService;

        private int _captchaId;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Captcha);

            var coins = Intent.GetStringExtra("Coins");
            var type = Intent.GetStringExtra("Type");

            await Initialise(coins, type);
            RegisterControls();
        }

        private async Task Initialise(string coins, string type)
        {
            var lblCaptchaClaim = FindViewById<TextView>(Resource.Id.lblCaptchaClaim);
            lblCaptchaClaim.Text = $"Enter the text in the given image below to claim {coins} {type} coins";

            _captchaService = new CaptchaService();

            await LoadCaptcha();
        }
        private async Task LoadCaptcha()
        {
            var image = await _captchaService.GetCaptcha();

            _captchaId = image.CaptchaId;

            SetImage(image.Image);
        }

        public void SetImage(string stream)
        {
            var image = stream.Substring(10);
            var imageBytes = Convert.FromBase64String(image);
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
            var captchaAnswer = FindViewById<TextView>(Resource.Id.txtCaptcha);

            var result = await _captchaService.CaptchaSubmit(
                    new CaptchaAnswerRequest(_captchaId, captchaAnswer.Text.ToUpper(), App.GameId));

            if (result.Code == CaptchaCodes.Valid)
            {
                GoToMenu();
            }
            else
            {
                Toast.MakeText(this, "Please try again. .", ToastLength.Long).Show();
                //await LoadCaptcha();
            }
        }

        public void GoToMenu()
        {
            var menuIntent = new Intent(this, typeof(MenuActivity));
            StartActivity(menuIntent);
        }
    }
}