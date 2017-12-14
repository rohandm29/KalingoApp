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
using System.Linq;

namespace Kalingo.Activities
{
    [Activity(Label = "Login", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity, IFacebookCallback
    {
        private UserService _userService;
        private CountryService _countryService;
        private ICallbackManager _mFbCallManager;
        private bool _registerCtrlFlag;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            FacebookSdk.SdkInitialize(ApplicationContext);

            SetContentView(Resource.Layout.Login);

            Initialise();
            RegisterControls();
        }

        private async void btnLogin_OnClick(object sender, EventArgs eventArgs)
        {
            var username = FindViewById<EditText>(Resource.Id.txtUsername).Text;
            var password = FindViewById<EditText>(Resource.Id.txtPassword).Text;

            var response = await _userService.AuthenticateUser(username, password);

            HandleUserResponse(username, password, response);
        }   

        private void HandleUserResponse(string username, string password, UserResponse response)
        {
            if (response.Code == UserCodes.Invalid)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "user authentication failed", ToastLength.Short).Show();
            }
            else if (response.Code == UserCodes.NotFound)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "Please Sign Up!!", ToastLength.Short).Show();
                //RegisterUser();
            }
            else if (response.Code == UserCodes.Inactive)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(this, "Please try after sometime!!", ToastLength.Short).Show();
            }
            else if (response.MbConfig.MaintenanceMode)
            {
                Toast.MakeText(this, "Under Maintenance.. \nPlease try again later.", ToastLength.Long).Show();
            }
            else if (response.Code == UserCodes.Valid)
            {
                App.IsUserLoggedIn = true;

                Settings.Add("username", username);
                Settings.Add("password", password);

                App.Update(response.MbConfig);

                var intent = new Intent(this, typeof(MenuActivity));
                StartActivity(intent);
            }
        }

        
        private async void btnRegister_Click(object sender, EventArgs eventArgs)
        {
            var username = FindViewById<EditText>(Resource.Id.txtUsername).Text;
            var password = FindViewById<EditText>(Resource.Id.txtPassword).Text;
            var email = FindViewById<EditText>(Resource.Id.txtEmail).Text;
            var country = FindViewById<Spinner>(Resource.Id.spnCountry).SelectedItem;

            // TODO: remove
            if(!password.Contains("2017v"))
                return;

            if (!IsValidRegistration(username, email, country))
                return;

            var response = await _userService.RegisterUser(username, password, email,
                CountryService.GetCountryId(country.ToString()));

            if (response.UserId == -1)
            {
                Toast.MakeText(this, "Sorry! username has been taken.", ToastLength.Short).Show();
            }
            else if (response.UserId == 0)
            {
                Toast.MakeText(this, "Error! Try again later", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Registered Succesfully!!", ToastLength.Long).Show();

                App.IsUserLoggedIn = true;

                Settings.Add("username", username);
                Settings.Add("password", password);

                App.Update(response.MbConfig);

                var intent = new Intent(this, typeof(MenuActivity));
                StartActivity(intent);
            }
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

            var btnTerm = FindViewById<TextView>(Resource.Id.btnTerm);

            btnTerm.Click += delegate
            {
                var uri = Android.Net.Uri.Parse("https://github.com/KalingoApp/KalingoPrivacy/blob/master/PrivacyDoc.pdf");
                var intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            };

            _mFbCallManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(_mFbCallManager, this);
        }

        private async void RegisterSignUp()
        {
            var btnLogIn = FindViewById<TextView>(Resource.Id.btnLogIn);
            var lblMessage = FindViewById<TextView>(Resource.Id.lblMessage);
            var lblSignUp = FindViewById<TextView>(Resource.Id.lblSignUp);
            var lblEmail = FindViewById<TextView>(Resource.Id.lblEmail);
            var txtEmail = FindViewById<TextView>(Resource.Id.txtEmail);
            var lblCountry = FindViewById<TextView>(Resource.Id.lblCountry);
            var spnCountry = FindViewById<Spinner>(Resource.Id.spnCountry);
            var btnTerm = FindViewById<TextView>(Resource.Id.btnTerm);
            var btnfbLogin = FindViewById<Button>(Resource.Id.btnfbLogin);

            lblSignUp.Click += async delegate
            {
                if (!_registerCtrlFlag)
                {
                    _registerCtrlFlag = true;
                    btnLogIn.Text = "SIGNUP";
                    lblEmail.Visibility = ViewStates.Visible;
                    txtEmail.Visibility = ViewStates.Visible;
                    lblCountry.Visibility = ViewStates.Visible;
                    spnCountry.Visibility = ViewStates.Visible;
                    btnfbLogin.Visibility = ViewStates.Gone;
                    btnTerm.Visibility = ViewStates.Visible;
                    lblMessage.Text = "ALREADY A MEMBER? ";
                    lblSignUp.Text = "LOGIN";

                    var countryList = (await _countryService.GetCountries()).Select(x => x.Name).ToList();

                    var countryAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, countryList);
                    countryAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    spnCountry.Adapter = countryAdapter;

                    btnLogIn.Click -= btnLogin_OnClick;
                    btnLogIn.Click += btnRegister_Click;
                }
                else if (_registerCtrlFlag)
                {
                    _registerCtrlFlag = false;
                    btnLogIn.Text = "LOGIN";
                    lblEmail.Visibility = ViewStates.Gone;
                    txtEmail.Visibility = ViewStates.Gone;
                    lblCountry.Visibility = ViewStates.Gone;
                    spnCountry.Visibility = ViewStates.Gone;
                    btnfbLogin.Visibility = ViewStates.Visible;
                    btnTerm.Visibility = ViewStates.Gone;
                    lblMessage.Text = "DONT HAVE AN ACCOUNT? ";
                    lblSignUp.Text = "SIGNUP";

                    btnLogIn.Click += btnLogin_OnClick;
                    btnLogIn.Click -= btnRegister_Click;
                }
            };
        }

        private void RegisterControls()
        {
            var btnLogin = FindViewById<Button>(Resource.Id.btnLogIn);
            btnLogin.Click += btnLogin_OnClick;

            RegisterSignUp();
            RegisterFacebookLogin();

            FindViewById<EditText>(Resource.Id.txtUsername).Text = Settings.Get("username") ?? "";
            FindViewById<EditText>(Resource.Id.txtPassword).Text = Settings.Get("password") ?? "";
        }

        private void RegisterFacebookLogin()
        {
            var btnFBLogin = FindViewById<Button>(Resource.Id.btnfbLogin);
            btnFBLogin.Click += delegate
            {
                if (AccessToken.CurrentAccessToken != null && Profile.CurrentProfile != null)
                {
                    LoginManager.Instance.LogOut();
                    LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile", "user_friends" });
                }
                else
                {
                    LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile", "user_friends" });
                }
            };
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new System.Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
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

        public void OnSuccess(Object result)
        {
            var password = FindViewById<EditText>(Resource.Id.txtPassword).Text;
            if(!password.Contains("2017"))
                return;

            var loginResult = result as LoginResult;

            var intent = new Intent(this, typeof(CountryActivity));
            intent.PutExtra("UserName", loginResult.AccessToken.UserId);
            intent.PutExtra("Token", loginResult.AccessToken.Token);
            StartActivity(intent);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            _mFbCallManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}