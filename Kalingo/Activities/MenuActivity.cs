using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Gms.Ads;
using Android.Gms.Ads.Reward;
using Kalingo.AdMob;
using Kalingo.Core;
using Kalingo.Master;

namespace Kalingo.Activities
{
    [Activity(Label = "M E N U", MainLauncher = true)]
    public class MenuActivity : Activity, IRewardedVideoAdListener
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
            var btnPlayMinesBoom = FindViewById<Button>(Resource.Id.btnPlayMinesBoom);
            btnPlayMinesBoom.Click += BtnPlayMinesBoomOnClick;
            btnPlayMinesBoom.Text += "\n________________________\n Attempts Left : 3";

            var btnShopVouchers = FindViewById<Button>(Resource.Id.btnShopVouchers);
            btnShopVouchers.Text += $"\n________________________\n Gold Coins : {App.Gold}";
            btnShopVouchers.Click += BtnShopClick;

            var lblMyAccount = FindViewById<TextView>(Resource.Id.lblMyAccount);
            lblMyAccount.Clickable = true;
            lblMyAccount.Click += MyAccount_OnClick;
        }

        #region Rewarded Ad Handlers
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

        #endregion

        private void MyAccount_OnClick(object sender, EventArgs eventArgs)
        {
            var intent = new Intent(this, typeof(MyAccountActivity));
            StartActivity(intent);
        }

        #region  Unused

        private void LoadInterstistialAd()
        {
            _interstitialAdListener = new InterstitialAdListener(ApplicationContext);

            _interstitialAd = new InterstitialAd(this)
            {
                //AdUnitId = "ca-app-pub-3940256099942544/1033173712",
                AdUnitId = "ca-app-pub-7100837506775638/6637403349",  // prod interstitial ad
                AdListener = _interstitialAdListener,
            };

            _interstitialAd.LoadAd(new AdRequest.Builder().Build());
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

        #endregion
    }
}