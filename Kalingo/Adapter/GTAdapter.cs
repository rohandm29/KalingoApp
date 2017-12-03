using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.App;
using Android;

namespace Kalingo.Adapters
{

    public class GTAdapter : BaseAdapter<GridTicketImage>
    {
        private readonly Activity context;
        private readonly List<GridTicketImage> gridTicketImages;

        public GTAdapter(Activity context, List<GridTicketImage> gridTicketImages)
        {
            this.context = context;
            this.gridTicketImages = gridTicketImages;
        }

        public override GridTicketImage this[int position]
        {
            get
            {
                return gridTicketImages[position];
            }
        }

        public override int Count
        {
            get
            {
                return gridTicketImages.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.TicketCards, parent, false);
            }

            var imageView = view.FindViewById<ImageView>(Resource.Id.ticketImg);

            imageView.SetImageResource(gridTicketImages[position].image);

            return view;
        }
    }
}