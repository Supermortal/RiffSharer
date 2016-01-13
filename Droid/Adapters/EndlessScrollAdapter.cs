using System;
using System.Collections.Generic;
using Android.Widget;
using Android.App;
using Android.Views;
using Android.Support.V7.Widget;

namespace RiffSharer.Droid.Adapters
{
    public abstract class EndlessScrollAdapter<T> : RecyclerView.Adapter where T : Java.Lang.Object
    {

        public const int VIEW_TYPE_LOADING = 0;
        public const int VIEW_TYPE_ACTIVITY = 1;

        public List<T> DataList { get; set; }

        protected Activity _activity;
        
        protected int _progressLayoutId;

        private int _serverListSize = -1;

        public int ServerListSize { get { return _serverListSize; } set { _serverListSize = value; } }

        public override int ItemCount { get { return DataList.Count; } }

        public EndlessScrollAdapter(Activity activity, List<T> list, int progressLayoutId)
        {
            _activity = activity;
            DataList = list;
            _progressLayoutId = progressLayoutId;
        }

        public abstract override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType);

        public abstract override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position);

        public int GetViewTypeCount()
        {
            return 2;
        }

        public override int GetItemViewType(int position)
        {           
            return (position >= DataList.Count) ? VIEW_TYPE_LOADING
                            : VIEW_TYPE_ACTIVITY;
        }

        public override long GetItemId(int position)
        {
            return (GetItemViewType(position) == VIEW_TYPE_ACTIVITY) ? position
                            : -1;
        }

        public View GetFooterView(int position, View convertView,
                                  ViewGroup parent)
        {
            if (position >= _serverListSize && _serverListSize > 0)
            {
                // the ListView has reached the last row
                TextView tvLastRow = new TextView(_activity);
                //tvLastRow.SetHint(Resource.String.reached_the_last_row);
                tvLastRow.Gravity = GravityFlags.Center;
                return tvLastRow;
            }
        
            View row = convertView;
            if (row == null)
            {
                row = _activity.LayoutInflater.Inflate(
                    _progressLayoutId, parent, false);
            }
        
            return row;
        }

    }
}

