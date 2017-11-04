using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Gms.Ads;
using Android.Gms.Ads.Reward;
using Android.Runtime;
using Kalingo.AdMob;
using Kalingo.Core;
using Kalingo.Master;

namespace Kalingo.Activities
{
    [Activity(Label = "M E N U"/*, MainLauncher = true*/)]
    public class MenuActivity : BaseActivity, Android.Gms.Ads.Reward.IRewardedVideoAdListener
    {
        private InterstitialAd _interstitialAd;
        private InterstitialAdListener _interstitialAdListener;

        private InterstitialAd _interstitialAd1;
        private IRewardedVideoAd _rewardedVideoAd;
        private RewardedAdListener _rewardedAdListener;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Menu);

            RegisterControl();

            //LoadInterstistialAd();
            //LoadRewardedAd();
        }

        private void LoadInterstistialAd()
        {
            _interstitialAdListener = new InterstitialAdListener(this.ApplicationContext);

            _interstitialAd = new InterstitialAd(this)
            {
                //AdUnitId = "ca-app-pub-3940256099942544/1033173712",
                AdUnitId = "ca-app-pub-7100837506775638/6637403349",  // prod interstitial ad
                AdListener = _interstitialAdListener,
            };

            _interstitialAd.LoadAd(new AdRequest.Builder().Build());
        }

        public void LoadRewardedAd()
        {
            _rewardedAdListener = new RewardedAdListener(this.BaseContext);
            _rewardedVideoAd = MobileAds.GetRewardedVideoAdInstance(this);
            _rewardedVideoAd.RewardedVideoAdListener = this;

            // prod rewarded ad
           // _rewardedVideoAd.LoadAd("ca-app-pub-7100837506775638/6637403349", new AdRequest.Builder().Build());

            // test Adunit
            _rewardedVideoAd.LoadAd("ca-app-pub-3940256099942544/5224354917", new AdRequest.Builder().Build());
        }

        private void BtnPlayLaddersOnClick(object sender, EventArgs e)
        {
            if (_interstitialAd.IsLoaded)
                _interstitialAd.Show();
            else
            {
                Toast.MakeText(this, "Failed to load ad", ToastLength.Short).Show();
            }
        }

        private void BtnPlayMinesBoomOnClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MinesBoomActivity));
            StartActivity(intent);

            //if (_rewardedVideoAd.IsLoaded)
            //    _rewardedVideoAd.Show();
            //else
            //{
            //    Toast.MakeText(this, "Failed to load rewarded ad", ToastLength.Short).Show();
            //}
        }

        private void BtnShopClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ShopActivity));
            StartActivity(intent);
        }

        private void RegisterControl()
        {
            var btnPlayGiftBoxes = FindViewById<Button>(Resource.Id.btnPlayLadders);
            btnPlayGiftBoxes.Click += BtnPlayLaddersOnClick;

            var btnPlayMinesBoom = FindViewById<Button>(Resource.Id.btnPlayMinesBoom);
            btnPlayMinesBoom.Click += BtnPlayMinesBoomOnClick;

            var btnShop = FindViewById<Button>(Resource.Id.btnShop);
            btnShop.SetBackgroundResource(Resource.Drawable.Cart);
            btnShop.Click += BtnShopClick;

            var btnAccount = FindViewById<Button>(Resource.Id.btnAccount);
            btnAccount.SetBackgroundResource(Resource.Drawable.MyAccount);

            var lblCoinCount = FindViewById<TextView>(Resource.Id.lblCoinCount);
            lblCoinCount.Text = $"Gold: {App.Gold} | Silver: {App.Silver}";
        }

        public void OnRewarded(IRewardItem reward)
        {
            var intent = new Intent(this, typeof(MinesBoomActivity));
            StartActivity(intent);

            //intent.PutExtra("Reward", reward.Amount.ToString());
            //intent.PutExtra("Type", reward.Type);
        }

        public void OnRewardedVideoAdClosed()
        {
        }

        public void OnRewardedVideoAdFailedToLoad(int errorCode)
        {
            Toast.MakeText(this, $"FailedToLoad {errorCode}", ToastLength.Short).Show();
        }

        public void OnRewardedVideoAdLeftApplication()
        {
        }

        public void OnRewardedVideoAdLoaded()
        {
            Toast.MakeText(this, "AdLoaded", ToastLength.Short).Show();
        }

        public void OnRewardedVideoAdOpened()
        {
        }

        public void OnRewardedVideoStarted()
        {
        }
    }
}