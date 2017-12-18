using System;
using Android.Content;
using Android.Gms.Ads;
using Android.Widget;
using Kalingo.Activities;
using Kalingo.Core;

namespace Kalingo.AdMob
{
    public class InterstitialAdListener : AdListener
    {
        private readonly Context _context;
        private readonly Action _callBackOnInsterstitialLoaded;
        private readonly Action<int> _failedToLoadAd;

        public InterstitialAdListener(Context context, Action callBackOnInsterstitialLoaded, Action<int> failedToLoadAd)
        {
            _context = context;
            _callBackOnInsterstitialLoaded = callBackOnInsterstitialLoaded;
            _failedToLoadAd = failedToLoadAd;
        }

        public override void OnAdClosed()
        {
            var intent = new Intent(_context, typeof(MinesBoomActivity));
            intent.PutExtra("Reward", $"Insterstitial-{App.CountryId}");
            _context.StartActivity(intent);
        }

        public override void OnAdLeftApplication()
        {
        }

        public override void OnAdFailedToLoad(int errorCode)
        {
            _failedToLoadAd(errorCode);
        }

        public override void OnAdLoaded()
        {
            Toast.MakeText(_context, "Loaded", ToastLength.Short).Show();

            _callBackOnInsterstitialLoaded();
        }

        public override void OnAdOpened()
        {
        }

    }
}