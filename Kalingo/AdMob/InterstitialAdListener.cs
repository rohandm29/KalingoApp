using Android.Content;
using Android.Gms.Ads;
using Android.Widget;

namespace Kalingo.AdMob
{
    public class InterstitialAdListener : AdListener
    {
        private readonly Context _context;

        public InterstitialAdListener(Context context)
        {
            _context = context;
        }

        public override void OnAdClosed()
        {
            Toast.MakeText(_context, "OnAdClosed called", ToastLength.Short).Show();
            base.OnAdClosed();
        }

        public override void OnAdLeftApplication()
        {
            Toast.MakeText(_context, "OnAdLeftApplication called", ToastLength.Short).Show();
            base.OnAdLeftApplication();
        }

        public override void OnAdFailedToLoad(int errorCode)
        {
            Toast.MakeText(_context, "OnAdFailedToLoad called", ToastLength.Short).Show();
            base.OnAdLeftApplication();
        }

        public override void OnAdLoaded()
        {
            Toast.MakeText(_context, "OnAdLoaded called", ToastLength.Short).Show();
            base.OnAdLeftApplication();
        }

        public override void OnAdOpened()
        {
            Toast.MakeText(_context, "OnAdOpened called", ToastLength.Short).Show();
            base.OnAdLeftApplication();
        }

    }
}