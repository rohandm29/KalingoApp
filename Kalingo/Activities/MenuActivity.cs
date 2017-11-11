using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Gms.Ads;
using Android.Gms.Ads.Reward;
using Android.Views;
using Kalingo.AdMob;
using Kalingo.Core;
using Kalingo.Master;

namespace Kalingo.Activities
{
    [Activity(Label = "M E N U", MainLauncher = true) ]
    public class MenuActivity : Activity, IRewardedVideoAdListener
    {
        private Button _minesboom;

        private InterstitialAd _interstitialAd;
        private InterstitialAdListener _interstitialAdListener;

        private IRewardedVideoAd _rewardedVideoAd;

        private ProgressDialog _progress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Menu);

            LoadAd();

            RegisterControl();
        }

        private void SetupProgress()
        {
            _progress = new ProgressDialog(this) { Indeterminate = true };
            _progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progress.SetMessage("rolling..");
            _progress.SetCancelable(false);
        }

        private void LoadAd()
        {
            if (App.PromoUser != 0)
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
            //StartActivity(intent);

            ShowAd();
        }

        private void ShowAd()
        {
            var count = 0;

            if (App.PromoUser != 1)
            {
                //while (!_rewardedVideoAd.IsLoaded)
                //{
                //    if (count < 10)
                //    {
                //        count++;
                //        ShowMessage("...");
                //        Thread.Sleep(1000);
                //    }
                //    else
                //    {
                //        ShowMessage("There was a problem loading game. Please check your internet connection.");
                //        break;
                //    }
                //}
                if (_rewardedVideoAd.IsLoaded)
                {
                    _rewardedVideoAd.Show();
                }
            }
            else
            {
                while (!_interstitialAd.IsLoaded)
                {
                    if (count < 3)
                    {
                        Thread.Sleep(2000);
                        count++;
                    }
                    else
                    {
                        ShowMessage("There was a problem loading game. Please check your internet connection.");
                        break;
                    }
                }
                if (_interstitialAd.IsLoaded)
                {
                    _interstitialAd.Show();
                }
            }
        }

        private void ShowMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Short).Show();
        }

        private void BtnShopClick(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ShopActivity));
            StartActivity(intent);
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
            //var btnPlayMinesBoom = FindViewById<Button>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Enabled = true;

            var txtLoading = FindViewById<TextView>(Resource.Id.txtLoading);
            txtLoading.Visibility = ViewStates.Invisible;

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

        private void LoadRewardedAd()
        {
            _rewardedVideoAd = MobileAds.GetRewardedVideoAdInstance(this);
            _rewardedVideoAd.RewardedVideoAdListener = this;

            // prod rewarded ad
            // _rewardedVideoAd.LoadAd("ca-app-pub-7100837506775638/6637403349", new AdRequest.Builder().Build());

            // test Adunit
            _rewardedVideoAd.LoadAd("ca-app-pub-3940256099942544/5224354917", new AdRequest.Builder().Build());
        }

        private void LoadInterstistialAd()
        {
            _interstitialAdListener = new InterstitialAdListener(ApplicationContext);

            _interstitialAd = new InterstitialAd(this)
            {
                AdUnitId = "ca-app-pub-3940256099942544/1033173712",    // test ad
                //AdUnitId = "ca-app-pub-7100837506775638/6637403349",  // prod interstitial ad
                AdListener = _interstitialAdListener,
            };

            _interstitialAd.LoadAd(new AdRequest.Builder().Build());
        }

        private void RegisterControl()
        {
            _minesboom = FindViewById<Button>(Resource.Id.btnPlayMinesBoom);
            _minesboom.Click += BtnPlayMinesBoomOnClick;
            _minesboom.Text += "\n_____\n Attempts Left : 3";
            _minesboom.Enabled = false;

            var btnShopVouchers = FindViewById<Button>(Resource.Id.btnShopVouchers);
            btnShopVouchers.Text += $"\n_____\n Gold Coins : {App.Gold}";
            btnShopVouchers.Click += BtnShopClick;

            var lblMyAccount = FindViewById<TextView>(Resource.Id.lblMyAccount);
            lblMyAccount.Clickable = true;
            lblMyAccount.Click += MyAccount_OnClick;
        }

        #region  Unused

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