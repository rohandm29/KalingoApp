using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Kalingo.Api.Client.Services;

namespace Kalingo.Activities
{
    [Activity(Label = "Kalingo", MainLauncher = true, Icon = "@drawable/icon")]
    public class VoucherActivity : Activity
    {
        private VoucherService _voucherService;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Voucher);

            await Initialise();
            RegisterControl();
        }

        private async Task Initialise()
        {
            _voucherService = new VoucherService();
            var voucherResponse = await _voucherService.GetVouchers();
            var vouchersList = voucherResponse.Select(x => x.Name).ToList();
            var worthList = voucherResponse.Select(x => x.Worth.ToString()).ToList();

            var spnVoucher = FindViewById<Spinner>(Resource.Id.spnVoucher);
            var voucherAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, vouchersList);
            voucherAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnVoucher.Adapter = voucherAdapter;

            var spnWorth = FindViewById<Spinner>(Resource.Id.spnWorth);
            var worthAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, worthList);
            worthAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnWorth.Adapter = worthAdapter;
        }

        private void BtnShopBackClicked(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MenuActivity));
            StartActivity(intent);
        }

        private void RegisterControl()
        {
            var lblVoucherNote = FindViewById<TextView>(Resource.Id.lblVoucherNote);
            lblVoucherNote.Text = "Voucher will be sent to your email address";

            var btnPlayGiftBoxes = FindViewById<Button>(Resource.Id.btnShopBack);
            btnPlayGiftBoxes.Click += BtnShopBackClicked;
            
            LoadVoucherImages();
        }

        private void LoadVoucherImages()
        {
            var btnAmazon = FindViewById<ImageButton>(Resource.Id.btnAmazon);
            btnAmazon.SetImageResource(Resource.Drawable.Amazon);

            var btnEbay = FindViewById<ImageButton>(Resource.Id.btnEbay);
            btnEbay.SetImageResource(Resource.Drawable.Ebay);

            var btnMns = FindViewById<ImageButton>(Resource.Id.btnMns);
            btnMns.SetImageResource(Resource.Drawable.Mns);

            var btnBodyShop = FindViewById<ImageButton>(Resource.Id.btnBodyShop);
            btnBodyShop.SetImageResource(Resource.Drawable.BodyShop);

            var btnHnm = FindViewById<ImageButton>(Resource.Id.btnHnm);
            btnHnm.SetImageResource(Resource.Drawable.Hnm);

            var btnFlipkart = FindViewById<ImageButton>(Resource.Id.btnFlipkart);
            btnFlipkart.SetImageResource(Resource.Drawable.Flipkart);
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