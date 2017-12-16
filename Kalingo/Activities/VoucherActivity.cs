using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Kalingo.Adapter;
using Kalingo.Api.Client.Services;
using Kalingo.Games.Contract.Entity.Voucher;
using Kalingo.Adapters;
using Kalingo.Core;
using Kalingo.Games.Contract.Entity;

namespace Kalingo.Activities
{
    [Activity(Label = "Kalingo", /*MainLauncher = true,*/ Icon = "@drawable/icon")]
    public class VoucherActivity : Activity
    {
        private VoucherService _voucherService;
        private IEnumerable<VoucherResponse> _voucherResponse;
        private List<string> _vouchersList = new List<string>();

        private GridView _gridView;
        private GTAdapter _adapter;
        private List<GridTicketImage> _gridTicketImages;

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
            _vouchersList = new List<string> {"SELECT VOUCHER"};
            _vouchersList.AddRange(_voucherResponse.Select(x => x.Description).ToList());

            var spnVoucher = FindViewById<Spinner>(Resource.Id.spnVoucher);
            spnVoucher.ItemSelected += Voucher_OnSelected;

            //var voucherAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, vouchersList);
            //voucherAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spnVoucher.Adapter = voucherAdapter;

            var customspinnerAdapter = new CustomSpinnerAdapter(this, _vouchersList);
            spnVoucher.Adapter = customspinnerAdapter;

            _gridView = FindViewById<GridView>(Resource.Id.alltickets);
            _adapter = new GTAdapter(this, GridTicketImages());
            _gridView.Adapter = _adapter;
        }

        private void Voucher_OnSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position != 0)
            {
                Spinner spinner = (Spinner)sender;

                var sel = spinner.GetItemAtPosition(e.Position).ToString();
                var cost = _voucherResponse.First(x => x.Description == sel).Coins;

                var lblVoucherCost = FindViewById<TextView>(Resource.Id.lblVoucherCost);
                lblVoucherCost.Text = $"COINS NEED : {cost} GOLD.   AVAILABE : {App.Gold}";

                FindViewById<Button>(Resource.Id.btnClaimVoucher).Enabled = true;
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

            if (claimResponse.Code == VoucherCodes.Valid)
            {
                Toast.MakeText(this, "Voucher claimed!!", ToastLength.Long).Show();
                App.Gold -= _voucherResponse.First(x => x.Id == id).Worth;
                return;
            }

            Toast.MakeText(this,"Sorry. Try again later.", ToastLength.Long).Show();
        }

        private int GetVoucherId(string voucher)
        {
            return _voucherResponse.First(x => x.Description == voucher).Id;
        }

        private void RegisterControl()
        {
            var btnPlayGiftBoxes = FindViewById<Button>(Resource.Id.btnShopBack);
            btnPlayGiftBoxes.Click += BtnShopBackClicked;

            var btnClaimVoucher = FindViewById<Button>(Resource.Id.btnClaimVoucher);
            btnClaimVoucher.Click += Claim_Clicked;
            btnClaimVoucher.Enabled = false;

            var lblVoucherCost = FindViewById<TextView>(Resource.Id.lblVoucherCost);
            lblVoucherCost.Text = $"Voucher will be sent to : {App.EmailAddress}";
        }

        private List<GridTicketImage> GridTicketImages()
        {
            _gridTicketImages = new List<GridTicketImage>();

            var gridImage = new GridTicketImage(Resource.Drawable.Amazon);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.Ebay);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.Mns);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.walmart);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.Flipkart);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.BMS);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.Myntra);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.groupon);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.Ebay);
            _gridTicketImages.Add(gridImage);

            gridImage = new GridTicketImage(Resource.Drawable.BodyShop);
            _gridTicketImages.Add(gridImage);

            return _gridTicketImages;
        }
    }
}