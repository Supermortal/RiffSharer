
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

using Supermortal.Common.PCL.Helpers;

using RiffSharer.Services.Abstract;

using RiffSharer.Droid.Adapters;
using RiffSharer.Droid.Models;

namespace RiffSharer.Droid
{
    class HomeFragment : Android.Support.V4.App.Fragment
    {

        private readonly IRiffService _as;

        private RecyclerView _audioList;

        private AudioListAdapter _adapter;
        private int? _serverListCount = -1;
        private List<DroidRiff> _riffs;
        private LinearLayoutManager _manager;

        public HomeFragment()
            : this(IoCHelper.Instance.GetService<IRiffService>())
        {

        }

        public HomeFragment(IRiffService audioService)
        {
            _as = audioService;
        }

        #region Lifecycle

        public async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var success = await GetRiffs().ConfigureAwait(false);

            if (!success)
            {
                //                if (_titleText != null)
                //                    _titleText.Text = _activity.GetString(Resource.String.an_error_has_occured);
                return;
            }

            if (_audioList != null)
                SetListAdapter();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.HomeFragment, null);
            return root;
        }

        public override void OnResume()
        {
            base.OnResume();

            SetViews();
            SetHandlers();
        }

        #endregion

        #region Helpers

        private void SetViews()
        {
            _audioList = Activity.FindViewById<RecyclerView>(Resource.Id.homeAudioList);
            _manager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            _audioList.SetLayoutManager(_manager);

            if (_riffs != null)
                SetListAdapter();
        }

        private void SetHandlers()
        {
            
        }

        private async Task<bool> GetRiffs()
        {
            _serverListCount = await _as.GetRiffCountForUser("a1d9be8f-0b1c-4663-aecd-a9d76e11c124").ConfigureAwait(false);

            //            if (_serverListCount == null)
            //                return false;

            var riffs = await _as.GetRiffsForUser("a1d9be8f-0b1c-4663-aecd-a9d76e11c124", 1, 10).ConfigureAwait(false);

            if (riffs == null)
                return false;

            _riffs = riffs.Select(i => new DroidRiff(i)).ToList();

            return true;
        }

        private void SetListAdapter()
        {           
            _adapter = new AudioListAdapter(Activity, _riffs);
            _adapter.ServerListSize = (int)_serverListCount;
            _adapter.RiffClicked += RiffClicked_List;

            _audioList.AddOnScrollListener(new AudioScrollListener(_as, _adapter, Activity, _manager));
            _audioList.SetAdapter(_adapter);
        }

        #endregion

        #region Events

        private void RiffClicked_List(object sender, RiffClickedEventArgs e)
        {
            
        }

        #endregion

        #region Static

        public static Android.Support.V4.App.Fragment newInstance(Context context)
        {
            var busrouteFragment = new HomeFragment();
            return busrouteFragment;
        }

        #endregion
    }
}

