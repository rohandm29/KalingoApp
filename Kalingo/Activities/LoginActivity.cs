using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Kalingo.Api.Client.Services;
using Kalingo.Core;
using Kalingo.Helpers;
using Object = Java.Lang.Object;

namespace Kalingo.Activities
{
    [Activity(Label = "Login",/* MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        public UserService UserService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Login);

            Initialise();
            RegisterControls();
        }
        
        private  async void btnSubmit_OnClick(object sender, EventArgs eventArgs)
        {
            var username = FindViewById<EditText>(Resource.Id.txtUserName).Text;
            var password = FindViewById<EditText>(Resource.Id.txtPassword).Text;

            var intent = new Intent(this, typeof(MenuActivity));
            intent.PutExtra("credentials", username + ";" + password);

            if (await UserService.AuthenticateUser(username, password))
            {
                App.IsUserLoggedIn = true;

                Settings.Add("username", username);
                Settings.Add("password", password);

                StartActivity(intent);
            }
            else
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "user authentication failed", ToastLength.Short).Show();
            }
        }

        private void lblNewUser_OnClick(object sender, EventArgs eventArgs)
        {
            var lblNewUser = FindViewById<TextView>(Resource.Id.lblNewUser);
            lblNewUser.Clickable = true;
            lblNewUser.Text = "Login";

            lblNewUser.Click -= lblNewUser_OnClick;
            lblNewUser.Click += Login_OnClick;

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

            if (await UserService.RegisterUser(username, password, email, country.ToString()) != -1)
            {
                App.IsUserLoggedIn = true;
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Sorry! username has been taken.", ToastLength.Short).Show();
            }
        }

        private void Login_OnClick(object sender, EventArgs eventArgs)
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
            UserService = new UserService();
        }

        private void RegisterUser()
        {
            var btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            btnSubmit.Text = "Register User";
            btnSubmit.Click -= btnSubmit_OnClick;
            btnSubmit.Click += btnRegister_Click;

            ShowRegistration(ViewStates.Visible);
        }

        private void ShowRegistration(ViewStates state)
        {
            var lblemail = FindViewById<GridLayout>(Resource.Id.newUserLayout);
            lblemail.Visibility = state;

            var spnCountry = FindViewById<Spinner>(Resource.Id.spnCountry);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.country_array, Android.Resource.Layout.SimpleSpinnerItem);
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

