using System;
using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Support.V7.Widget;

using RiffSharer.Models;

using RiffSharer.Droid.Models;

namespace RiffSharer.Droid.Adapters
{
    public class AudioListAdapter : EndlessScrollAdapter<DroidAudio>
    {

        public AudioListAdapter(Activity activity, List<DroidAudio> list, int progressLayoutId)
            : base(activity, list, progressLayoutId)
        {

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.AudioListItem, parent, false);

            return new AudioViewHolder(itemView);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            
        }
    }

    class AudioViewHolder : RecyclerView.ViewHolder
    {

        //        public TextView Title { get; set; }
        //
        //        public TextView IPAddress { get; set; }
        //
        //        public TextView DateCreated { get; set; }


        public AudioViewHolder(View itemView)
            : base(itemView)
        {
//            Title = itemView.FindViewById<TextView>(Resource.Id.LeadListItemTitleText);
//            IPAddress = itemView.FindViewById<TextView>(Resource.Id.LeadListItemIPAddressText);
//            DateCreated = itemView.FindViewById<TextView>(Resource.Id.LeadListItemDateCreatedText);
        }

    }
}

