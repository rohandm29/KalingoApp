using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Kalingo.Core;

namespace Kalingo.Activities
{
    [Activity(Label = "Kalingo", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!App.IsUserLoggedIn)
            {
                var loginIntent = new Intent(this, typeof(LoginActivity));
                StartActivity(loginIntent);
            }
            else
            {
                var menuIntent = new Intent(this, typeof(MenuActivity));
                StartActivity(menuIntent);
            }
        }
    }
}