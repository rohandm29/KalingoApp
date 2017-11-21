using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Kalingo.Api.Client.Services;
using Kalingo.Games.Contract.Entity.Voucher;

namespace Kalingo.Activities
{
    [Activity(Label = "Kalingo", MainLauncher = true, Icon = "@drawable/icon")]
    public class VoucherActivity : Activity
    {
        private VoucherService _voucherService;
        private IEnumerable<VoucherResponse> _voucherResponse;

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
            _voucherResponse = await _voucherService.GetVouchers();
            var vouchersList = new List<string> {"Select Voucher"};
            vouchersList.AddRange(_voucherResponse.Select(x => x.Description).ToList());

            var spnVoucher = FindViewById<Spinner>(Resource.Id.spnVoucher);
            spnVoucher.ItemSelected += Voucher_OnSelected;

            var voucherAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, vouchersList);
            voucherAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnVoucher.Adapter = voucherAdapter;
        }

        private void Voucher_OnSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != 0)
            {
                Spinner spinner = (Spinner)sender;

                var sel = spinner.GetItemAtPosition(e.Position).ToString();
                var cost = _voucherResponse.First(x => x.Description == sel).Coins;

                var lblVoucherCost = FindViewById<TextView>(Resource.Id.lblVoucherCost);
                lblVoucherCost.Text = $"COINS NEED : {cost} GOLD";
            }
        }

        private void BtnShopBackClicked(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MenuActivity));
            StartActivity(intent);
        }

        private async void Claim_Clicked(object sender, EventArgs e)
        {
            var voucher = FindViewById<Spinner>(Resource.Id.spnVoucher).SelectedItem;
            var id = GetVoucherId(voucher.ToString());

            var claimResponse = await _voucherService.ClaimVoucher(id);

            Toast.MakeText(this, $"{claimResponse.Error.First()}", ToastLength.Long).Show();
        }

        private int GetVoucherId(string voucher)
        {
            return _voucherResponse.First(x => x.Description == voucher).Id;
        }

        private void RegisterControl()
        {
            var lblVoucherNote = FindViewById<TextView>(Resource.Id.lblVoucherNote);
            lblVoucherNote.Text = "Voucher will be sent to your email address";

            var btnPlayGiftBoxes = FindViewById<Button>(Resource.Id.btnShopBack);
            btnPlayGiftBoxes.Click += BtnShopBackClicked;

            var btnClaimVoucher = FindViewById<Button>(Resource.Id.btnClaimVoucher);
            btnClaimVoucher.Click += Claim_Clicked;

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
    }
}