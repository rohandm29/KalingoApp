using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Threading.Tasks;
using Kalingo.Api.Client.Services;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.User;
using Kalingo.Helpers;
using Object = Java.Lang.Object;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using System.Collections.Generic;

namespace Kalingo.Activities
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity, IFacebookCallback
    {
        private UserService _userService;
        private CountryService _countryService;
        private ICallbackManager _mFbCallManager;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FacebookSdk.SdkInitialize(ApplicationContext);

            SetContentView(Resource.Layout.Login);

            Initialise();
            RegisterControls();
        }

        private async void btnSubmit_OnClick(object sender, EventArgs eventArgs)
        {
            var username = FindViewById<EditText>(Resource.Id.txtUsername).Text;
            var password = FindViewById<EditText>(Resource.Id.txtPassword).Text;

            var response = await _userService.AuthenticateUser(username, password);

            HandleUserResponse(username, password, response);
        }   

        private void HandleUserResponse(string username, string password, UserResponse response)
        {
            if (response.MbConfig.MaintenanceMode)
            {
                Toast.MakeText(this, "Under Maintenance.. \nPlease try again later.", ToastLength.Long).Show();
            }
            else if (response.Code == UserCodes.Valid)
            {
                App.IsUserLoggedIn = true;

                Settings.Add("username", username);
                Settings.Add("password", password);

                UpdateSettings(response.MbConfig);

                var intent = new Intent(this, typeof(MenuActivity));
                StartActivity(intent);
            }
            else if(response.Code == UserCodes.Invalid)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "user authentication failed", ToastLength.Short).Show();
            }
            else if(response.Code == UserCodes.NotFound)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "Please Register!!", ToastLength.Short).Show();
                RegisterUser();
            }
            else if(response.Code == UserCodes.Inactive)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "Please try after sometime!!", ToastLength.Short).Show();
            }
        }

        private static void UpdateSettings(Config config)
        {
            App.Update(config.TotalChances, config.TotalGifts, config.InterstitialMode,
                config.MaintenanceMode, config.PlayAgainEnabled);
        }

        private void lblNewUser_OnClick(object sender, EventArgs eventArgs)
        {
            RegisterUser();
        }

        private async void btnRegister_Click(object sender, EventArgs eventArgs)
        {
            //var username = FindViewById<EditText>(Resource.Id.txtUsername).Text;
            //var password = FindViewById<EditText>(Resource.Id.txtPassword).Text;
            //var email = FindViewById<EditText>(Resource.Id.txtEmail).Text;
            //var country = FindViewById<Spinner>(Resource.Id.spnCountry).SelectedItem;

            //if (!IsValidRegistration(username, email, country))
            //    return;

            //var response = await _userService.RegisterUser(username, password, email,
            //    CountryService.GetCountryId(country.ToString()));

            //if (response == -1)
            //{
            //    Toast.MakeText(this, "Sorry! username has been taken.", ToastLength.Short).Show();
            //}
            //if (response == 0)
            //{
            //    Toast.MakeText(this, "Error! Try again later", ToastLength.Short).Show();
            //}
            //else
            //{
            //    Toast.MakeText(this, "Registered Succesfully!! \nPlease Login", ToastLength.Long).Show();
            //    ShowLogin_OnClick(new object(), new EventArgs());

            //    //App.IsUserLoggedIn = true;
            //    //var intent = new Intent(this, typeof(MenuActivity));
            //    //StartActivity(intent);
            //}
        }

        private async void RegisterFacebookUser(string userName, string token)
        {
            var userResponse = await _userService.FbUserLogin(userName, token);
        }

        private void ShowLogin_OnClick(object sender, EventArgs eventArgs)
        {
            var btnSubmit = FindViewById<Button>(Resource.Id.btnLogIn);
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            _userService = new UserService();
            _countryService = new CountryService();

            _mFbCallManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(_mFbCallManager, this);
        }

        private void RegisterUser()
        {
            var lblNewUser = FindViewById<TextView>(Resource.Id.lblNewUser);
            lblNewUser.Clickable = true;
            lblNewUser.Text = "Login";

            var btnLogIn = FindViewById<Button>(Resource.Id.btnLogIn);
            btnLogIn.Text = "Register User";
            btnLogIn.Click -= btnSubmit_OnClick;
            btnLogIn.Click += btnRegister_Click;

            lblNewUser.Click -= lblNewUser_OnClick;
            lblNewUser.Click += ShowLogin_OnClick;

            ShowRegistration(ViewStates.Visible);
        }

        private async void ShowRegistration(ViewStates state)
        {
            var lblemail = FindViewById<GridLayout>(Resource.Id.newUserLayout);
            lblemail.Visibility = state;

            //var countryList = (await _countryService.GetCountries()).Select(x => x.Name).ToList();

            //var spnCountry = FindViewById<Spinner>(Resource.Id.spnCountry);
            //var countryAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, countryList);
            //countryAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spnCountry.Adapter = countryAdapter;
        }

        private void RegisterControls()
        {
            var btnSubmit = FindViewById<Button>(Resource.Id.btnLogIn);
            btnSubmit.Click += btnSubmit_OnClick;
            btnSubmit.Text = "Login";

            //ShowRegistration(ViewStates.Invisible);

            var lblSignUp = FindViewById<TextView>(Resource.Id.lblSignUp);
            lblSignUp.Clickable = true;
            lblSignUp.Click += lblNewUser_OnClick;
            //lblSignUp.Text = "New User";

            var btnFBLogin = FindViewById<Button>(Resource.Id.btnfbLogin);
            btnFBLogin.Click += delegate
            {
                if (AccessToken.CurrentAccessToken != null && Profile.CurrentProfile != null)
                {
                    //user is logged in through facebook
                    LoginManager.Instance.LogOut();
                    //StartActivity(typeof(TicketsActivity));
                    LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile", "user_friends" });
                }
                else
                {
                    //the user is not logged in
                    LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile", "user_friends" });
                }
            };

            FindViewById<EditText>(Resource.Id.txtUsername).Text = Settings.Get("username") ?? "";
            FindViewById<EditText>(Resource.Id.txtPassword).Text = Settings.Get("password") ?? "";
        }
        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new System.Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new System.Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as System.Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {
            try
            {
                const string errorFileName = "Fatal.log";
                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = $"Time: {DateTime.Now}\r\nError: Unhandled Exception\r\n{exception}";
                File.WriteAllText(errorFilePath, errorMessage);

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        }

        public void OnCancel() { }

        public void OnError(FacebookException error) { }

        public async void OnSuccess(Object result)
        {
            LoginResult loginResult = result as LoginResult;

            var response = await _userService.FbUserLogin(loginResult.AccessToken.UserId, loginResult.AccessToken.Token);

            HandleUserResponse(string.Empty, string.Empty, response);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            _mFbCallManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}