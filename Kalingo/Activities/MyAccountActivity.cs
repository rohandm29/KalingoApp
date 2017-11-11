using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Kalingo.Api.Client.Services;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity.User;
using Kalingo.Master;

namespace Kalingo.Activities
{
    [Activity(Label = "My Account", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
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

            var spnCountry = FindViewById<Spinner>(Resource.Id.spnCountry);

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.country_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnCountry.Adapter = adapter;
        }

        private async void btnSubmit_OnClick(object sender, EventArgs eventArgs)
        {
            var txtEmail = FindViewById<EditText>(Resource.Id.txtUpdateEmail).Text;
            var country = FindViewById<Spinner>(Resource.Id.spnCountry).SelectedItem;

            await _userService.UpdateUser(txtEmail, country.ToString());
        }
    }
}