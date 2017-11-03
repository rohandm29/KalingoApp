using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Widget;
using Kalingo.Api.Client.Services;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity.MinesBoom;
using Kalingo.Master;

namespace Kalingo.Activities
{
    [Activity(Label = "MinesBoomActivity"/*, MainLauncher = true*/)]
    public class MinesBoomActivity : BaseActivity
    {
        private readonly MinesBoomService _minesBoomService = new MinesBoomService();
        private MediaPlayer _playerRed, _playerGreen;
        private ProgressDialog _progress;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MinesBoom);

            SetupProgress();
            SetupAudio();
            RegisterControls();

            await _minesBoomService.CreateMinesBoom(1);

            ShowDialogCoinsEarned();
        }

        private async void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            _progress.Show();   

            var btnSelected = (Button)sender;
            var id = int.Parse(btnSelected.Text);

            var result = await _minesBoomService.Submit(id);

            //System.Threading.Thread.Sleep(2000);
            
            btnSelected.Text = "";

            btnSelected.SetBackgroundResource(result.SelectionCorrect ? Resource.Drawable.GreenGift : Resource.Drawable.RedGift);

            SetText(result.TotalChances, result.TotalGifts);

            IsGameOver(result);

            PlaySound(result.SelectionCorrect);

            _progress.Dismiss();
        }

        private void PlaySound(bool win)
        {
            if (win)
                _playerGreen.Start();
            else
                _playerRed.Start();
        }

        private async void IsGameOver(MinesBoomGameResult result)
        {
            if (result.HasWon)
            {
                Toast.MakeText(this, "Game Won..", ToastLength.Long).Show();

                var txtMinesChances = FindViewById<TextView>(Resource.Id.txtMinesChances);
                txtMinesChances.Text = "you have won 5 Gold coins. . ";
                txtMinesChances.SetTypeface(null, TypefaceStyle.BoldItalic);
            }
            else
            {
                if (result.TotalChances == 0)
                    Toast.MakeText(this, "Game lost..", ToastLength.Long).Show();
                else
                    return;
            }

            await Task.Delay(2000);

            var menuIntent = new Intent(this, typeof(MenuActivity));
            StartActivity(menuIntent);
        }

        private void ShowDialogCoinsEarned()
        {
            if (!App.CoinsDialogShown)
            {
                RunOnUiThread(() =>
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Coins Earned On Wins");
                    builder.SetMessage("     5 Gifts = 5 Gold \n\n     4 Gift = 2 Gold \n\n     3 Gifts =  5 Silver");
                    builder.SetCancelable(false);
                    builder.SetNeutralButton("OK", delegate { DismissDialog(builder); });
                    builder.Show();
                });

                App.CoinsDialogShown = true;
            }
        }

        public void DismissDialog(AlertDialog.Builder builder)
        {
            builder.Dispose();
        }

        private void SetText(int chances, int gifts)
        {
            var txtMinesChances = FindViewById<TextView>(Resource.Id.txtMinesChances);
            txtMinesChances.Text = $"Gift - {gifts} | lives remaining - {chances} | watch mines!!";
            txtMinesChances.SetTypeface(null, TypefaceStyle.Bold);
        }

        private void SetupAudio()
        {
            _playerRed = MediaPlayer.Create(this, Resource.Raw.Red);
            _playerGreen = MediaPlayer.Create(this, Resource.Raw.Win);
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
            txtMinesChances.Text = $"Gift - {5} | lives remaining - {7} | watch mines!!";
            txtMinesChances.SetTypeface(null, TypefaceStyle.Bold);
        }
    }
}