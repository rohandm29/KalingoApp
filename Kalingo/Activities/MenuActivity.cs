using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Gms.Ads;
using Android.Gms.Ads.Reward;
using Kalingo.AdMob;
using Kalingo.Api.Client.Services;
using Kalingo.Core;

namespace Kalingo.Activities
{
    [Activity(Label = "M E N U", MainLauncher = true)]
    public class MenuActivity : Activity, IRewardedVideoAdListener
    {
        private UserService _userService;
        private ImageView _minesboom;

        private InterstitialAd _interstitialAd;
        private InterstitialAdListener _interstitialAdListener;

        private IRewardedVideoAd _rewardedVideoAd;
        private int _playCount;
        private static bool _interstitialLoaded;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);

            LoadAd();
            await Initialise();
            RegisterControl();
        }

        private async Task Initialise()
        {
            _userService = new UserService();
            _playCount = await _userService.GetUserLimit();
        }

        private void LoadAd()
        {
            // Banner ad prod - ca-app-pub-7100837506775638/2856509156, test - ca-app-pub-3940256099942544/6300978111
            var mAdView = FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            mAdView.LoadAd(adRequest);

            // Video ad
            if (App.MixedMode)
            {
                LoadMixedAd();
            }
            else if (App.InterstitialMode || App.PromoUser == 1)
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
            //var intent = new Intent(this, typeof(MinesBoomActivity));
            //intent.PutExtra("Reward", $"Insterstitial-{App.CountryId}");
            //StartActivity(intent);

            ShowAd();
        }

        private void ShowAd()
        {
            if (App.MixedMode)
            {
                if(_interstitialLoaded && _interstitialAd.IsLoaded)
                    _interstitialAd.Show();

                else if(_rewardedVideoAd.IsLoaded)
                {
                    _rewardedVideoAd.Show();
                }
            }
            else if (App.InterstitialMode || App.PromoUser == 1)
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
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }
        private void ShowMessageShort(string message)
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
            intent.PutExtra("Reward", reward.Type + "-" + reward.Amount);
            //intent.PutExtra("Type", reward.Type);
            StartActivity(intent);
        }

        public void OnRewardedVideoAdClosed()
        {
            ShowMessage("Minesboom will be enabled on completion of the Advert");
            TryEnableMinesboom();
        }

        public void OnRewardedVideoAdFailedToLoad(int errorCode)
        {
            HandleFailedToLoadAd(errorCode);
        }

        private void HandleFailedToLoadAd(int errorCode)
        {
            _minesboom = FindViewById<ImageView>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Click -= BtnPlayMinesBoomOnClick;
            _minesboom.Click += Refresh_Clicked;

            TryEnableMinesboom();

            FindViewById<TextView>(Resource.Id.lblPlayMinesboom).Text = "REFRESH";
            FindViewById<TextView>(Resource.Id.txtLoading).Text = "Failed To Load. Try refreshing.";
            ShowMessageShort($"Error!! {errorCode}");
        }

        private void Refresh_Clicked(object sender, EventArgs eventArgs)
        {
            _minesboom.Enabled = false;
            LoadAd();

            _minesboom = FindViewById<ImageView>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Click -= Refresh_Clicked;
            _minesboom.Click += BtnPlayMinesBoomOnClick;

            FindViewById<TextView>(Resource.Id.txtLoading).Text = "Loading...Please wait";
        }

        public void OnRewardedVideoAdLoaded()
        {
            FindViewById<TextView>(Resource.Id.lblPlayMinesboom).Text = $"Play Minesboom\n_____\n Plays Left : { _playCount}";
            TryEnableMinesboom();

            var txtLoading = FindViewById<TextView>(Resource.Id.txtLoading);
            txtLoading.Text = "Loaded..";
        }

        private void TryEnableMinesboom()
        {
            if(_playCount > 0)
                _minesboom.Enabled = true;
            else
            {
                FindViewById<TextView>(Resource.Id.txtLoading).Text = "Total number of plays per day are exhausted";
            }
        }

        public void OnRewardedVideoAdOpened()
        {
        }

        public void OnRewardedVideoStarted()
        {
        }

        public void OnRewardedVideoAdLeftApplication()
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
             _rewardedVideoAd.LoadAd(App.RewardedAdUnit, new AdRequest.Builder().Build());

            // test Adunit
            //_rewardedVideoAd.LoadAd("ca-app-pub-3940256099942544/5224354917", new AdRequest.Builder().Build());
        }

        private void LoadMixedAd()
        {
            if (MinesBoomHelper.GetRandomFlag())
            {
                LoadInterstistialAd();
                _interstitialLoaded = true;
            }
            else
            {
                LoadRewardedAd();
                _interstitialLoaded = false;
            }
        }

        private void LoadInterstistialAd()
        {
            _interstitialAdListener = new InterstitialAdListener(ApplicationContext, CallBack_OnInsterstitial_Loaded, HandleFailedToLoadAd);

            _interstitialAd = new InterstitialAd(this)
            {
                //AdUnitId = "ca-app-pub-3940256099942544/1033173712",    // test ad
                AdUnitId = App.InterstitialAdUnit,  // prod interstitial ad
                AdListener = _interstitialAdListener,
            };

            _interstitialAd.LoadAd(new AdRequest.Builder().Build());
        }

        public void CallBack_OnInsterstitial_Loaded()
        {
            FindViewById<TextView>(Resource.Id.lblPlayMinesboom).Text = $"Play Minesboom\n_____\n Plays Left : { _playCount}";
            TryEnableMinesboom();

            var txtLoading = FindViewById<TextView>(Resource.Id.txtLoading);
            txtLoading.Text = "Loaded..";
        }

        private void RegisterControl()
        {
            _minesboom = FindViewById<ImageView>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Click += BtnPlayMinesBoomOnClick;
            _minesboom.Enabled = false;

            var lblPlayMinesboom = FindViewById<TextView>(Resource.Id.lblPlayMinesboom);
            lblPlayMinesboom.Click += BtnPlayMinesBoomOnClick;
            lblPlayMinesboom.Text += $"\n_____\n Plays Left : {_playCount}";

            var btnShopVouchers = FindViewById<ImageView>(Resource.Id.btnShopVouchers);
            btnShopVouchers.Click += BtnShopClick;

            var lblMyAccount = FindViewById<TextView>(Resource.Id.lblMyAccount);
            lblMyAccount.Clickable = true;
            lblMyAccount.Click += MyAccount_OnClick;

            var txtCoins = FindViewById<TextView>(Resource.Id.txtCoins);
            txtCoins.Text = $"Gold - {App.Gold}  |  Silver - {App.Silver}  |  Bronze - {App.Bronze}";
        }
    }
}