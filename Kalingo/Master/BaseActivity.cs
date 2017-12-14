using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Kalingo.Master
{
    [Activity(Label = "BaseActivity")]
    public class BaseActivity : Activity
    {
        ImageView image;
        LinearLayout linBase;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.SetContentView(Resource.Layout.BaseActivity);

            image = FindViewById<ImageView>(Resource.Id.imgHeader);

            linBase = (LinearLayout)FindViewById(Resource.Id.linBase);
        }

        public override void SetContentView(int id)
        {
            LayoutInflater inflater = (LayoutInflater) BaseContext.GetSystemService(LayoutInflaterService);
            inflater.Inflate(id, linBase);
        }
    }
}