using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using Kalingo.Api.Client.Services;

namespace Kalingo.Activities
{
    [Activity(Label = "My Account", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class MyAccountActivity : Activity
    {
        private UserService _userService;
        private CountryService _countryService;

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
            _countryService = new CountryService();
        }

        private async void RegisterControls()
        {
            var btnSubmit = FindViewById<Button>(Resource.Id.btnAccountSubmit);
            btnSubmit.Click += btnSubmit_OnClick;

            var countryList = (await _countryService.GetCountries()).Select(x => x.Name).ToList();
            
            var spnCountry = FindViewById<Spinner>(Resource.Id.spnCountry);
            var countryAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, countryList);
            countryAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCountry.Adapter = countryAdapter;
        }

        private async void btnSubmit_OnClick(object sender, EventArgs eventArgs)
        {
            var txtEmail = FindViewById<EditText>(Resource.Id.txtUpdateEmail).Text;
            var country = FindViewById<Spinner>(Resource.Id.spnCountry).SelectedItem.ToString();

            await _userService.UpdateUser(txtEmail, CountryService.GetCountryId(country));
        }
    }
}