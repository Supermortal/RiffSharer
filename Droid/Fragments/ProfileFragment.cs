
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
    class ProfileFragment: Android.Support.V4.App.Fragment
    {

        private readonly IAudioService _as;

        private RecyclerView _audioList;

        private AudioListAdapter _adapter;
        private int? _serverListCount = -1;
        private List<DroidAudio> _audios;
        private LinearLayoutManager _manager;

        public ProfileFragment()
            : this(IoCHelper.Instance.GetService<IAudioService>())
        {

        }

        public ProfileFragment(IAudioService audioService)
        {
            _as = audioService;
        }

        #region Lifecycle

        public async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var success = await GetAudios().ConfigureAwait(false);

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
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.ProfileFragment, null);
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
            _audioList = Activity.FindViewById<RecyclerView>(Resource.Id.profileAudioList);
            _manager = new LinearLayoutManager(Activity);
            _audioList.SetLayoutManager(_manager);

            if (_audios != null)
                SetListAdapter();
        }

        private void SetHandlers()
        {
            
        }

        private void SetListAdapter()
        {           
            _adapter = new AudioListAdapter(Activity, _audios);
            _adapter.ServerListSize = (int)_serverListCount;
            _audioList.AddOnScrollListener(new AudioScrollListener(_as, _adapter, Activity, _manager));
            _audioList.SetAdapter(_adapter);

//            _adapter = new LeadsListAdapter(_activity, _leads);
//            _adapter.ServerListSize = (int)_serverListSize;
//            _list.AddOnScrollListener(new LeadsScrollListener(_sws, _adapter, _activity, _manager));
//            _list.SetAdapter(_adapter);
//
//            _titleText.Text = _activity.GetString(Resource.String.total_count);
//            _countText.Text = (_serverListSize == null) ? "0" : _serverListSize.ToString();
        }

        private async Task<bool> GetAudios()
        {
            _serverListCount = await _as.GetAudioCountForUser("a1d9be8f-0b1c-4663-aecd-a9d76e11c124").ConfigureAwait(false);

//            if (_serverListCount == null)
//                return false;

            var audios = await _as.GetAudiosForUser("a1d9be8f-0b1c-4663-aecd-a9d76e11c124", 1, 10).ConfigureAwait(false);

            if (audios == null)
                return false;

            _audios = audios.Select(i => new DroidAudio(i)).ToList();

            return true;
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

