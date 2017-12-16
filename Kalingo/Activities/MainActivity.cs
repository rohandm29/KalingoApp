using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Kalingo.Core;

namespace Kalingo.Activities
{
    [Activity(Label = "Kalingo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += HandleExceptions;

            AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;

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
        void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
        {
            e.Handled = true;
            Console.Write("HANDLED EXCEPTION");
        }

        static void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            //Exception d = (Exception)e.ExceptionObject;
            Console.WriteLine("TEST");
        }
    }
}