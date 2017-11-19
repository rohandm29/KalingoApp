using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Kalingo.Api.Client.Services;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.User;
using Kalingo.Helpers;
using Object = Java.Lang.Object;

namespace Kalingo.Activities
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        private UserService _userService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Login);

            Initialise();
            RegisterControls();
        }

        private async void btnSubmit_OnClick(object sender, EventArgs eventArgs)
        {
            var username = FindViewById<EditText>(Resource.Id.txtUserName).Text;
            var password = FindViewById<EditText>(Resource.Id.txtPassword).Text;

            var response = await _userService.AuthenticateUser(username, password);

            HandleUserResponse(username, password, response);
        }

        private void HandleUserResponse(string username, string password, UserResponse response)
        {
            if (response.Code == UserCodes.Valid)
            {
                App.IsUserLoggedIn = true;

                Settings.Add("username", username);
                Settings.Add("password", password);

                var intent = new Intent(this, typeof(MenuActivity));
                StartActivity(intent);
            }
            if (response.Code == UserCodes.Invalid)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "user authentication failed", ToastLength.Short).Show();
            }
            if (response.Code == UserCodes.NotFound)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "Please Register!!", ToastLength.Short).Show();
                RegisterUser();
            }
            if (response.Code == UserCodes.Inactive)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "Please try after sometime!!", ToastLength.Short).Show();
            }
        }

        private void lblNewUser_OnClick(object sender, EventArgs eventArgs)
        {
            RegisterUser();
        }

        private async void btnRegister_Click(object sender, EventArgs eventArgs)
        {
            var username = FindViewById<EditText>(Resource.Id.txtUserName).Text;
            var password = FindViewById<EditText>(Resource.Id.txtPassword).Text;
            var email = FindViewById<EditText>(Resource.Id.txtEmail).Text;
            var country = FindViewById<Spinner>(Resource.Id.spnCountry).SelectedItem;

            if (!IsValidRegistration(username, email, country))
                return;

            var intent = new Intent(this, typeof(MenuActivity));
            intent.PutExtra("credentials", username + ";" + password);

            var response = await _userService.RegisterUser(username, password, email, country.ToString());

            if (response == -1)
            {
                Toast.MakeText(this, "Sorry! username has been taken.", ToastLength.Short).Show();
            }
            if (response == 0)
            {
                Toast.MakeText(this, "Error! Try again later", ToastLength.Short).Show();
            }
            else
            {
                App.IsUserLoggedIn = true;
                StartActivity(intent);
            }
        }

        private void ShowLogin_OnClick(object sender, EventArgs eventArgs)
        {
            var btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnSubmit.Click -= btnRegister_Click;
            RegisterControls();
        }

        private bool IsValidRegistration(string username, string email, Object country)
        {
            if (string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(country.ToString())
                || string.Equals(country.ToString(), "Select"))
            {
                Toast.MakeText(this, "Please select country", ToastLength.Short).Show();
                return false;
            }

            return true;
        }

        private void Initialise()
        {
            _userService = new UserService();
        }

        private void RegisterUser()
        {
            var lblNewUser = FindViewById<TextView>(Resource.Id.lblNewUser);
            lblNewUser.Clickable = true;
            lblNewUser.Text = "Login";

            var btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnSubmit.Text = "Register User";
            btnSubmit.Click -= btnSubmit_OnClick;
            btnSubmit.Click += btnRegister_Click;

            lblNewUser.Click -= lblNewUser_OnClick;
            lblNewUser.Click += ShowLogin_OnClick;

            ShowRegistration(ViewStates.Visible);
        }

        private void ShowRegistration(ViewStates state)
        {
            var lblemail = FindViewById<GridLayout>(Resource.Id.newUserLayout);
            lblemail.Visibility = state;

            var spnCountry = FindViewById<Spinner>(Resource.Id.spnCountry);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.country_array,
                Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCountry.Adapter = adapter;
        }

        private void RegisterControls()
        {
            var btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnSubmit.Click += btnSubmit_OnClick;
            btnSubmit.Text = "Login";

            ShowRegistration(ViewStates.Invisible);

            var lblNewUser = FindViewById<TextView>(Resource.Id.lblNewUser);
            lblNewUser.Clickable = true;
            lblNewUser.Click += lblNewUser_OnClick;
            lblNewUser.Text = "New User";

            FindViewById<EditText>(Resource.Id.txtUserName).Text = Settings.Get("username") ?? "";
            FindViewById<EditText>(Resource.Id.txtPassword).Text = Settings.Get("password") ?? "";
        }
    }
}