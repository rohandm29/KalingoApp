using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Kalingo.Api.Client.Services;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;

namespace Kalingo.Activities
{
    [Activity(Label = "Kalingo", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class CountryActivity : Activity
    {
        private readonly UserService _userService = new UserService();
        private readonly CountryService _countryService = new CountryService();
        private string _userId;
        private string _token;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _userId = Intent.GetStringExtra("UserId");
            _token = Intent.GetStringExtra("Token");

            var response = await _userService.AddFbUser(_userId, _token);

            if (response.Code == UserCodes.Valid)
            {
                App.IsUserLoggedIn = true;

                var menuIntent = new Intent(this, typeof(MenuActivity));
                StartActivity(menuIntent);
            }
            else
            {
                SetContentView(Resource.Layout.Country);

                var spnCountry = FindViewById<Spinner>(Resource.Id.spnCountry);
                var countryList = (await _countryService.GetCountries()).Select(x => x.Name).ToList();
                var countryAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, countryList);
                countryAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                spnCountry.Adapter = countryAdapter;

                var btnGo = FindViewById<Button>(Resource.Id.btnGo);
                btnGo.Click += BtnGo_Click;
            }
        }

        private async void BtnGo_Click(object sender, EventArgs e)
        {
            var country = FindViewById<Spinner>(Resource.Id.spnCountry).SelectedItem;
            var countryId = CountryService.GetCountryId(country.ToString()));

            var response = await _userService.AddFbUser(_userId, _token, countryId);

            App.IsUserLoggedIn = true;

            var menuIntent = new Intent(this, typeof(MenuActivity));
            StartActivity(menuIntent);
        }
    }
}