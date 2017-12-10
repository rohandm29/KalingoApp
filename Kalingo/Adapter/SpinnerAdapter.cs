using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Object = Java.Lang.Object;

namespace Kalingo.Adapter
{
    public class CustomSpinnerAdapter : BaseAdapter
    {
        private readonly Context activity;
        private List<string> asr;

        public CustomSpinnerAdapter(Context context, List<string> asr)
        {
            this.asr = asr;
            activity = context;
        }
        public override int Count => asr.Count;

        public override Object GetItem(int position)
        {
            return asr[position];
        }
        public override long GetItemId(int position)
        {
            return (long)position;
        }
        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            var item = asr.ElementAtOrDefault(position);
            TextView txt = new TextView(this.activity);
            txt.SetPadding(20, 20, 0, 20);
            txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 15);
            //txt.SetForegroundGravity((GravityFlags.CenterVertical));
            txt.Gravity = GravityFlags.Center;
            txt.Text = item;
            txt.SetTextColor(Android.Graphics.Color.White);
            txt.SetBackgroundColor(Android.Graphics.Color.Rgb(43, 86, 150));
            return txt;
        }
        public override View GetView(int i, View view, ViewGroup viewgroup)
        {
            var item = asr.ElementAtOrDefault(i);
            var txt = new TextView(activity) {Gravity = GravityFlags.Center};
            txt.SetTextSize(Android.Util.ComplexUnitType.Sp, 15);
            txt.SetCompoundDrawablesRelativeWithIntrinsicBounds(0, 0, Resource.Drawable.dropdown_arrow, 0);
            txt.SetPaddingRelative(20, 3, 30, 3);
            txt.Text = item;
            txt.SetTextColor(Android.Graphics.Color.White);
            txt.SetBackgroundColor(Android.Graphics.Color.Rgb(43, 86, 150));
            return txt;
        }
    }
}