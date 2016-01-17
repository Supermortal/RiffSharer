using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Android.Widget;
using Android.App;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Graphics;

using Supermortal.Common.Droid.Helpers;

using RiffSharer.Models;

using RiffSharer.Droid.Models;

namespace RiffSharer.Droid.Adapters
{

    public delegate void RiffClickedEventHandler(object sender,RiffClickedEventArgs e);

    public class AudioListAdapter : EndlessScrollAdapter<DroidRiff>
    {

        public event RiffClickedEventHandler RiffClicked;

        public AudioListAdapter(Activity activity, List<DroidRiff> list)
            : base(activity, list, Resource.Layout.ListProgress)
        {

        }

        public override void OnBindViewHolder(Android.Support.V7.Widget.RecyclerView.ViewHolder holder, int position)
        {
            var h = (AudioViewHolder)holder;

            if (position < DataList.Count)
            {
                var data = DataList[position];

                h.Name.Text = data.Name;
                h.Duration.Text = data.DurationSeconds.ToString();
                h.DateCreated.Text = data.DateCreated.ToLongDateString();

                h.MainView.Click += (object sender, EventArgs e) =>
                {
                    if (RiffClicked != null)
                        RiffClicked(sender, new RiffClickedEventArgs() { RiffID = data.RiffID });
                };

                (new TaskFactory()).StartNew(() =>
                    {
                        var bmp = BitmapHelper.LoadAndResizeBitmap(_activity.Resources, Resource.Drawable.music, 50, 50);

                        _activity.RunOnUiThread(() =>
                            {
                                h.Thumbnail.SetImageBitmap(bmp);               
                                bmp.Dispose();
                            });
                    });                        
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.AudioListItem, parent, false);

            return new AudioViewHolder(itemView);
        }
    }

    class AudioViewHolder : RecyclerView.ViewHolder
    {

        public TextView Name { get; set; }

        public TextView Duration { get; set; }

        public TextView DateCreated { get; set; }

        public ImageView Thumbnail { get; set; }

        public CardView MainView { get; set; }


        public AudioViewHolder(View itemView)
            : base(itemView)
        {
            Name = itemView.FindViewById<TextView>(Resource.Id.audioListItemName);
            Duration = itemView.FindViewById<TextView>(Resource.Id.audioListItemDuration);
            DateCreated = itemView.FindViewById<TextView>(Resource.Id.audioListItemDateCreated);
            Thumbnail = itemView.FindViewById<ImageView>(Resource.Id.audioListItemImage);
            MainView = itemView.FindViewById<CardView>(Resource.Id.audioListCardView);
        }

    }

    public class RiffClickedEventArgs : EventArgs
    {
        public string RiffID { get; set; }
    }
}

