using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Gms.Ads;
using Android.Gms.Ads.Reward;
using Android.Views;
using Kalingo.AdMob;
using Kalingo.Api.Client.Services;
using Kalingo.Core;

namespace Kalingo.Activities
{
    [Activity(Label = "M E N U"  /*,MainLauncher = true */  )]
    public class MenuActivity : Activity, IRewardedVideoAdListener
    {
        private UserService _userService;
        private Button _minesboom;

        private InterstitialAd _interstitialAd;
        private InterstitialAdListener _interstitialAdListener;

        private IRewardedVideoAd _rewardedVideoAd;
        private int _playCount;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);

            LoadAd();
            Initialise();
            RegisterControl();
        }

        private async void Initialise()
        {
            _userService = new UserService();
            _playCount = await _userService.GetUserLimit(App.UserId);
        }

        private void LoadAd()
        {
            if (App.InterstitialMode || App.PromoUser == 1)
            {
                LoadInterstistialAd();
            }
            else
            {
                LoadRewardedAd();
            }
        }

        private void BtnPlayMinesBoomOnClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MinesBoomActivity));
            StartActivity(intent);

            //ShowAd();
        }

        private void ShowAd()
        {
            if (App.InterstitialMode || App.PromoUser == 1)
            {
                if (_interstitialAd.IsLoaded)
                {
                    _interstitialAd.Show();
                }
            }
            else
            {
                if (_rewardedVideoAd.IsLoaded)
                {
                    _rewardedVideoAd.Show();
                }
            }
        }

        private void ShowMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        private void BtnShopClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(VoucherActivity));
            StartActivity(intent);
        }

        #region Rewarded Ad Handlers
        public void OnRewarded(IRewardItem reward)
        {
            var intent = new Intent(this, typeof(MinesBoomActivity));
            //intent.PutExtra("Reward", reward.Amount.ToString());
            //intent.PutExtra("Type", reward.Type);
            StartActivity(intent);
        }

        public void OnRewardedVideoAdClosed()
        {
            ShowMessage("Minesboom will be enabled on completion of the Advert");
        }

        public void OnRewardedVideoAdFailedToLoad(int errorCode)
        {
            _minesboom = FindViewById<Button>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Text = "Refresh";
            _minesboom.Enabled = true;
            _minesboom.Click -= BtnPlayMinesBoomOnClick;
            _minesboom.Click += Refresh_Clicked;

            ShowMessage($"Failed To Load {errorCode}. Try refreshing.");
        }

        private void Refresh_Clicked(object sender, EventArgs eventArgs)
        {
            LoadAd();

            _minesboom = FindViewById<Button>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Text = $"Play Minesboom \n_____\n Attempts Left : {_playCount}";
            _minesboom.Enabled = false;
            _minesboom.Click -= Refresh_Clicked;
            _minesboom.Click += BtnPlayMinesBoomOnClick;
        }

        public void OnRewardedVideoAdLeftApplication()
        {
        }

        public void OnRewardedVideoAdLoaded()
        {
            //var btnPlayMinesBoom = FindViewById<Button>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Enabled = true;

            var txtLoading = FindViewById<TextView>(Resource.Id.txtLoading);
            txtLoading.Visibility = ViewStates.Invisible;

            ShowMessage("AdLoaded");
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

        private void LoadRewardedAd()
        {
            _rewardedVideoAd = MobileAds.GetRewardedVideoAdInstance(this);
            _rewardedVideoAd.RewardedVideoAdListener = this;

            //prod rewarded ad
             _rewardedVideoAd.LoadAd("ca-app-pub-7100837506775638/6637403349", new AdRequest.Builder().Build());

            // test Adunit
            //_rewardedVideoAd.LoadAd("ca-app-pub-3940256099942544/5224354917", new AdRequest.Builder().Build());
        }

        private void LoadInterstistialAd()
        {
            _interstitialAdListener = new InterstitialAdListener(ApplicationContext, CallBack_OnInsterstitial_Loaded);

            _interstitialAd = new InterstitialAd(this)
            {
                //AdUnitId = "ca-app-pub-3940256099942544/1033173712",    // test ad
                AdUnitId = "ca-app-pub-7100837506775638/6637403349",  // prod interstitial ad
                AdListener = _interstitialAdListener,
            };

            _interstitialAd.LoadAd(new AdRequest.Builder().Build());
        }

        public void CallBack_OnInsterstitial_Loaded()
        {
            _minesboom.Enabled = true;

            var txtLoading = FindViewById<TextView>(Resource.Id.txtLoading);
            txtLoading.Visibility = ViewStates.Invisible;
        }

        private void RegisterControl()
        {
            _minesboom = FindViewById<Button>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Click += BtnPlayMinesBoomOnClick;
            _minesboom.Text += $"\n_____\n Attempts Left : {_playCount}";
            _minesboom.Enabled = false;

            var btnShopVouchers = FindViewById<Button>(Resource.Id.btnShopVouchers);
            btnShopVouchers.Text += $"\n_____\n Gold Coins : {App.Gold}";
            btnShopVouchers.Click += BtnShopClick;

            var lblMyAccount = FindViewById<TextView>(Resource.Id.lblMyAccount);
            lblMyAccount.Clickable = true;
            lblMyAccount.Click += MyAccount_OnClick;
        }
    }
}