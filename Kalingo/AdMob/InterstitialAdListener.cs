using System;
using Android.Content;
using Android.Gms.Ads;
using Android.Widget;
using Kalingo.Activities;

namespace Kalingo.AdMob
{
    public class InterstitialAdListener : AdListener
    {
        private readonly Context _context;
        private readonly Action _callBackOnInsterstitialLoaded;

        public InterstitialAdListener(Context context, Action callBackOnInsterstitialLoaded)
        {
            _context = context;
            _callBackOnInsterstitialLoaded = callBackOnInsterstitialLoaded;
        }

        public override void OnAdClosed()
        {
            var intent = new Intent(_context, typeof(MinesBoomActivity));
            _context.StartActivity(intent);
        }

        public override void OnAdLeftApplication()
        {
        }

        public override void OnAdFailedToLoad(int errorCode)
        {
        }

        public override void OnAdLoaded()
        {
            Toast.MakeText(_context, "Interstitial Loaded", ToastLength.Short).Show();

            _callBackOnInsterstitialLoaded();
        }

        public override void OnAdOpened()
        {
        }

    }
}