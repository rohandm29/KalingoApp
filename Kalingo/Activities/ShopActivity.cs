using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Kalingo.Core;
using Kalingo.Master;

namespace Kalingo.Activities
{
    [Activity(Label = "Kalingo", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class ShopActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Shop);

            RegisterControl();
        }

        private void BtnShopBackClicked(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MenuActivity));
            StartActivity(intent);
        }

        private void RegisterControl()
        {
            var lblVoucherHeader = FindViewById<TextView>(Resource.Id.lblVoucherHeader);
            lblVoucherHeader.Text = $"Voucher - 10 Gold coins.{System.Environment.NewLine} Click the voucher to get it.";

            var btnPlayGiftBoxes = FindViewById<Button>(Resource.Id.btnShopBack);
            btnPlayGiftBoxes.Click += BtnShopBackClicked;

            var btnAmazon = FindViewById<ImageButton>(Resource.Id.btnAmazon);
            btnAmazon.SetImageResource(Resource.Drawable.Amazon);
            btnAmazon.Click += Voucher_Click;

            var btnEbay = FindViewById<ImageButton>(Resource.Id.btnEbay);
            btnEbay.SetImageResource(Resource.Drawable.Ebay);
            btnEbay.Click += Voucher_Click;

            var btnMns = FindViewById<ImageButton>(Resource.Id.btnMns);
            btnMns.SetImageResource(Resource.Drawable.Mns);
            btnMns.Click += Voucher_Click;

            var btnBodyShop = FindViewById<ImageButton>(Resource.Id.btnBodyShop);
            btnBodyShop.SetImageResource(Resource.Drawable.BodyShop);
            btnBodyShop.Click += Voucher_Click;

            var btnHnm = FindViewById<ImageButton>(Resource.Id.btnHnm);
            btnHnm.SetImageResource(Resource.Drawable.Hnm);
            btnHnm.Click += Voucher_Click;

            var btnFlipkart = FindViewById<ImageButton>(Resource.Id.btnFlipkart);
            btnFlipkart.SetImageResource(Resource.Drawable.Flipkart);
            btnFlipkart.Click += Voucher_Click;
        }

        private void Voucher_Click(object sender, EventArgs e)
        {
            var btnSelected = (ImageButton)sender;
            var name = GetVoucherName(btnSelected.Id);

            Toast.MakeText(this, $"{name}'s Voucher will be sent to your email address", ToastLength.Long).Show();
        }

        private string GetVoucherName(int id)
        {
            switch (id)
            {
                case Resource.Id.btnAmazon: return "Amazon";
                case Resource.Id.btnEbay: return "Ebay";
                case Resource.Id.btnMns: return "Mark&Spencer";
                case Resource.Id.btnBodyShop: return "BodyShop";
                case Resource.Id.btnHnm: return "HnM";
                case Resource.Id.btnFlipkart: return "Flipkart";
                default: return "";
            }
        }
    }
}