using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Widget;
using Java.Lang;
using Kalingo.Api.Client.Services;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity.MinesBoom;

namespace Kalingo.Activities
{
    [Activity(Label = "MinesBoomActivity", /*MainLauncher = true,*/ ScreenOrientation = ScreenOrientation.Portrait)]
    public class MinesBoomActivity : Activity
    {
        private readonly MinesBoomService _minesBoomService = new MinesBoomService();
        private MediaPlayer _playerRed, _playerGreen;
        private ProgressDialog _progress;
        private bool _playAgain;
        private IEnumerable<Settings> _settings;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MinesBoom);

            LoadAd();
            SetupProgress();
            SetupAudio();
            RegisterControls();

            App.Reward = Intent.GetStringExtra("Reward");

            _settings = await _minesBoomService.CreateMinesBoom(App.UserId, false);

            ShowDialogCoinsEarned();
        }

        private void LoadAd()
        {
            // Banner ad prod - ca-app-pub-7100837506775638/2856509156
            var mAdView = FindViewById<AdView>(Resource.Id.adView);
            var adRequest = new AdRequest.Builder().Build();
            mAdView.LoadAd(adRequest);
        }

        private async void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            _progress.Show();

            var btnSelected = (Button) sender;
            btnSelected.Enabled = false;
            var id = int.Parse(btnSelected.Text);

            var result = await _minesBoomService.Submit(id, _playAgain);

            //System.Threading.Thread.Sleep(1000);

            btnSelected.Text = "";

            btnSelected.SetBackgroundResource(result.SelectionCorrect
                ? Resource.Drawable.GreenThumb
                : Resource.Drawable.RedGift);

            PlaySound(result.SelectionCorrect);

            ProcessResult(result);

            _progress.Dismiss();
        }

        private void PlaySound(bool win)
        {
            if (win)
                _playerGreen.Start();
            else
                _playerRed.Start();
        }

        private void ProcessResult(MinesboomSelectionResponse result)
        {
            if (result.TotalChances != 0)
            {
                SetText(result.TotalChances);
                return;
            }

            if (result.HasWon)
            {
                var txtMinesChances = FindViewById<TextView>(Resource.Id.txtMinesChances);
                var coinType = MinesBoomHelper.GetCoinType(result.CoinType);

                txtMinesChances.Text = $"You have won {result.CoinsWon} {coinType} coins.";
                txtMinesChances.SetTypeface(null, TypefaceStyle.BoldItalic);

                ShowMessage("Redirecting... please wait");

                GoToCaptcha(result.CoinsWon, coinType);
            }
            else if (result.TotalChances == 0)
            {
                ShowMessage("Nothing won");

                if (result.RandomSequence != null)
                    DisableAndShowMissedThumbs(result.RandomSequence);

                Thread.Sleep(1000);

                if (App.PlayAgainEnabled)
                    ShowDialogPlayAgain();
                else
                {
                    GoToMenu();

                    ShowMessage("Redirecting... please wait");
                }
            }
        }

        private async void PlayAgain()
        {
            _playAgain = true;

            _settings = await _minesBoomService.CreateMinesBoom(App.UserId, true);

            ClearScreen();
        }

        private void DisableAndShowMissedThumbs(string resultRandomSequence)
        {
            foreach (var id in resultRandomSequence.Split('-'))
            {
                var button = GetButton(int.Parse(id));
                button.Text = "";
                button.SetBackgroundResource(Resource.Drawable.GreenGift);
            }
            for (var i = 1; i <= 16; i++)
            {
                var button = GetButton(i);
                button.Enabled = false;
            }
        }

        private void ClearScreen()
        {
            SetContentView(Resource.Layout.MinesBoom);
            RegisterControls();
        }

        private void ShowDialogCoinsEarned()
        {
            if (!App.CoinsDialogShown)
            {
                RunOnUiThread(() =>
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("What Will You Win");
                    builder.SetMessage(GetCoinsWonMessage(false));
                    builder.SetCancelable(false);
                    builder.SetNeutralButton("OK", delegate { DismissDialog(builder); });
                    builder.Show();
                });

                App.CoinsDialogShown = true;
            }
        }

        private string GetCoinsWonMessage(bool playAgain)
        {
            var message = new StringBuilder();

            foreach (var setting in _settings.Where(x=>x.PlayAgain == playAgain))
            {
                message.Append($"{setting.MinesCount} Gifts = {setting.CoinCount} {MinesBoomHelper.GetCoinType(setting.CoinTypeId)} \n\n");
            }

            return message.ToString();
        }

        private void ShowDialogPlayAgain()
        {
            if (_playAgain)
            {
                _playAgain = false;

                RunOnUiThread(() =>
                {
                    var builder = new AlertDialog.Builder(this);
                    builder.SetTitle("What Will You Win");
                    builder.SetMessage(GetCoinsWonMessage(true));
                    builder.SetCancelable(false);
                    builder.SetPositiveButton("Play Again", delegate { PlayAgain(builder); });
                    builder.SetNeutralButton("Back", delegate { GoBack(builder); });
                    builder.Show();
                });
            }
            else
            {
                GoToMenu();
            }
        }

        public void DismissDialog(AlertDialog.Builder builder)
        {
            builder.Dispose();
        }

        public void GoBack(AlertDialog.Builder builder)
        {
            builder.Dispose();
            GoToMenu();
        }

        public void PlayAgain(AlertDialog.Builder builder)
        {
            PlayAgain();
        }

        public void GoToMenu()
        {
            var menuIntent = new Intent(this, typeof(MenuActivity));
            StartActivity(menuIntent);
        }

        public void GoToCaptcha(int coins, string type)
        {
            var menuIntent = new Intent(this, typeof(CaptchaActivity));
            menuIntent.PutExtra("Coins", coins);
            menuIntent.PutExtra("Type", type);
            StartActivity(menuIntent);
        }

        private void SetText(int chances)
        {
            var txtMinesChances = FindViewById<TextView>(Resource.Id.txtMinesChances);
            txtMinesChances.Text = $"Select any {chances} numbers to find gifts.";
            txtMinesChances.SetTypeface(null, TypefaceStyle.Bold);
        }

        private void SetupAudio()
        {
            _playerRed = MediaPlayer.Create(this, Resource.Raw.Red);
            _playerGreen = MediaPlayer.Create(this, Resource.Raw.Win);
        }

        private void ShowMessage(string message)
        {
            Toast.MakeText(this, message, ToastLength.Long).Show();
        }

        private void SetupProgress()
        {
            _progress = new ProgressDialog(this) { Indeterminate = true };
            _progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            _progress.SetMessage("rolling..");
            _progress.SetCancelable(false);
        }

        private void RegisterControls()
        {
            #region buttons click delgates
            var imgButton1 = FindViewById<Button>(Resource.Id.Button1);
            imgButton1.Click += ButtonOnClick;

            var imgButton2 = FindViewById<Button>(Resource.Id.Button2);
            imgButton2.Click += ButtonOnClick;

            var imgButton3 = FindViewById<Button>(Resource.Id.Button3);
            imgButton3.Click += ButtonOnClick;

            var imgButton4 = FindViewById<Button>(Resource.Id.Button4);
            imgButton4.Click += ButtonOnClick;

            var imgButton5 = FindViewById<Button>(Resource.Id.Button5);
            imgButton5.Click += ButtonOnClick;

            var imgButton6 = FindViewById<Button>(Resource.Id.Button6);
            imgButton6.Click += ButtonOnClick;

            var imgButton7 = FindViewById<Button>(Resource.Id.Button7);
            imgButton7.Click += ButtonOnClick;

            var imgButton8 = FindViewById<Button>(Resource.Id.Button8);
            imgButton8.Click += ButtonOnClick;

            var imgButton9 = FindViewById<Button>(Resource.Id.Button9);
            imgButton9.Click += ButtonOnClick;

            var imgButton10 = FindViewById<Button>(Resource.Id.Button10);
            imgButton10.Click += ButtonOnClick;

            var imgButton11 = FindViewById<Button>(Resource.Id.Button11);
            imgButton11.Click += ButtonOnClick;

            var imgButton12 = FindViewById<Button>(Resource.Id.Button12);
            imgButton12.Click += ButtonOnClick;

            var imgButton13 = FindViewById<Button>(Resource.Id.Button13);
            imgButton13.Click += ButtonOnClick;

            var imgButton14 = FindViewById<Button>(Resource.Id.Button14);
            imgButton14.Click += ButtonOnClick;

            var imgButton15 = FindViewById<Button>(Resource.Id.Button15);
            imgButton15.Click += ButtonOnClick;

            var imgButton16 = FindViewById<Button>(Resource.Id.Button16);
            imgButton16.Click += ButtonOnClick;

            #endregion

            var txtMinesChances = FindViewById<TextView>(Resource.Id.txtMinesChances);
            txtMinesChances.Text = $"Select any {App.TotalChances} numbers to find gifts.";
            txtMinesChances.SetTypeface(null, TypefaceStyle.Bold);
        }

        private Button GetButton(int buttonId)
        {
            switch (buttonId)
            {
                case 1: return FindViewById<Button>(Resource.Id.Button1);
                case 2: return FindViewById<Button>(Resource.Id.Button2);
                case 3: return FindViewById<Button>(Resource.Id.Button3);
                case 4: return FindViewById<Button>(Resource.Id.Button4);
                case 5: return FindViewById<Button>(Resource.Id.Button5);
                case 6: return FindViewById<Button>(Resource.Id.Button6);
                case 7: return FindViewById<Button>(Resource.Id.Button7);
                case 8: return FindViewById<Button>(Resource.Id.Button8);
                case 9: return FindViewById<Button>(Resource.Id.Button9);
                case 10: return FindViewById<Button>(Resource.Id.Button10);
                case 11: return FindViewById<Button>(Resource.Id.Button11);
                case 12: return FindViewById<Button>(Resource.Id.Button12);
                case 13: return FindViewById<Button>(Resource.Id.Button13);
                case 14: return FindViewById<Button>(Resource.Id.Button14);
                case 15: return FindViewById<Button>(Resource.Id.Button15);
                case 16: return FindViewById<Button>(Resource.Id.Button16);
            }

            return FindViewById<Button>(Resource.Id.Button1);
        }
    }
}