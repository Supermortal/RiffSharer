
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using RiffSharer.Repositories.Abstract;
using RiffSharer.Droid.Adapters;

namespace RiffSharer.Droid
{
    class ProfileFragment: Android.Support.V4.App.Fragment
    {

        private readonly ANonTableQueryAudioRepository _ar;

        private RecyclerView _audioList;

        private AudioListAdapter _adapter;

        public ProfileFragment()
        {

        }

        public ProfileFragment(ANonTableQueryAudioRepository ar)
        {
            _ar = ar;
        }

        #region Lifecycle

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.ProfileFragment, null);
            return root;
        }

        public override void OnResume()
        {
            base.OnResume();

            SetViews();
        }

        #endregion

        #region Helpers

        private void SetViews()
        {
            _audioList = Activity.FindViewById<RecyclerView>(Resource.Id.profileAudioList);
        }

        private void SetHandlers()
        {
            
        }

        private void SetListAdapter()
        {           
//            _adapter = new LeadsListAdapter(_activity, _leads);
//            _adapter.ServerListSize = (int)_serverListSize;
//            _list.AddOnScrollListener(new LeadsScrollListener(_sws, _adapter, _activity, _manager));
//            _list.SetAdapter(_adapter);
//
//            _titleText.Text = _activity.GetString(Resource.String.total_count);
//            _countText.Text = (_serverListSize == null) ? "0" : _serverListSize.ToString();
        }

        #endregion

        #region Static

        public static Android.Support.V4.App.Fragment newInstance(Context context)
        {
            var busrouteFragment = new ProfileFragment();
            return busrouteFragment;
        }

        #endregion
    }
}

