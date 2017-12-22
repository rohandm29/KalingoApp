using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Kalingo.Api.Client.Services;
using Kalingo.Core;

namespace Kalingo.Activities
{
    [Activity(Label = "My Account",/* MainLauncher = true, */Icon = "@drawable/icon")]
    public class MyAccountActivity : Activity
    {
        private UserService _userService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MyAccount);

            Initialise();
            RegisterControls();
        }

        private void Initialise()
        {
            _userService = new UserService();
        }

        private void RegisterControls()
        {
            var btnSubmit = FindViewById<Button>(Resource.Id.btnAccountSubmit);
            btnSubmit.Click += btnSubmit_OnClick;

            var lblCoins = FindViewById<TextView>(Resource.Id.lblCoins);
            lblCoins.Text = $"Gold Coins: {App.Gold} \nSilver Coins: {App.Silver} \nBronze Coins: {App.Bronze}";
            var lblCoinsMap = FindViewById<TextView>(Resource.Id.lblCoinsMap);
            lblCoinsMap.Text = ("(10 Silver = 1 Gold, 10 Broze = 1 Silver)");

            var lblLogout = FindViewById<TextView>(Resource.Id.lblLogout);
            lblLogout.Clickable = true;
            lblLogout.Click += LblLogout_Click; 
        }

        private void LblLogout_Click(object sender, EventArgs e)
        {
            App.ClearSession();

            var menuIntent = new Intent(this, typeof(MainActivity));
            StartActivity(menuIntent);
        }

        private async void btnSubmit_OnClick(object sender, EventArgs eventArgs)
        {
            var txtEmail = FindViewById<EditText>(Resource.Id.txtUpdateEmail).Text;

            var result = await _userService.UpdateUser(txtEmail);

            Toast.MakeText(this, result ? "Done!!" : "Please try later!", ToastLength.Short);
        }
    }
}