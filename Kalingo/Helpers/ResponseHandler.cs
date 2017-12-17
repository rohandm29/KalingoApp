using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Kalingo.Activities;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;
using Kalingo.Games.Contract.Entity.User;

namespace Kalingo.Helpers
{
    public class ResponseHandler 
    {
        public static bool HandleUserResponse(Context context, string username, string password, UserResponse response, bool saveCreds = true)
        {
            if (response.Code == UserCodes.Invalid)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(context, "user authentication failed", ToastLength.Short).Show();
            }
            else if (response.Code == UserCodes.NotFound)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(context, "Please Sign Up!!", ToastLength.Short).Show();
            }
            else if (response.Code == UserCodes.Inactive)
            {
                App.IsUserLoggedIn = false;
                Toast.MakeText(context, "Please try after sometime!!", ToastLength.Short).Show();
            }
            else if (response.MbConfig.MaintenanceMode)
            {
                App.IsUserLoggedIn = false;
                var message = response.Errors.Any() ? response.Errors.First() : "Under Maintenance.. \nPlease try again later.";
                Toast.MakeText(context, message, ToastLength.Long).Show();
            }
            else if (response.Code == UserCodes.Valid)
            {
                App.IsUserLoggedIn = true;

                Settings.Add("username", username);
                Settings.Add("password", password);

                App.Update(response.MbConfig);

                //var intent = new Intent(context, typeof(MenuActivity));
                //StartActivity(intent);
            }

            return App.IsUserLoggedIn;
        }
    }
}