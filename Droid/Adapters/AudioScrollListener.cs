using System;
using System.Threading.Tasks;
using System.Linq;

using Android.App;
using Android.Support.V7.Widget;

using RiffSharer.Services.Abstract;

using RiffSharer.Droid.Models;

namespace RiffSharer.Droid.Adapters
{
    public class AudioScrollListener : EndlessScrollListener
    {

        private IAudioService _as;
        private AudioListAdapter _adapter;
        private Activity _activity;

        public AudioScrollListener(IAudioService audioService, AudioListAdapter adapter, Activity activity, LinearLayoutManager manager)
            : base(manager)
        {
            _as = audioService;
            _adapter = adapter;
            _activity = activity;
        }

        public async override Task<bool> OnLoadMore(int page, int totalItemsCount)
        {
            var list = _as.GetAudiosForUser("a1d9be8f-0b1c-4663-aecd-a9d76e11c124", page, 10);
            var audios = list.Select(i => new DroidAudio(i));

            _activity.RunOnUiThread(() =>
                {
                    foreach (var audio in audios)
                    {
                        _adapter.DataList.Add(audio);
                    }
                    _adapter.NotifyDataSetChanged();
                });
            //            List<Lead> l = await _sws.GetLeads(FrissonLeadsPullerGlobal.AuthToken, page, 10).ConfigureAwait(false);
            //            var leads = l.Select(i => new DroidLead(i));
            //
            //            _activity.RunOnUiThread(() =>
            //                {
            //                    foreach (var lead in leads)
            //                    {
            //                        _adapter.DataList.Add(lead);
            //                    }
            //                    _adapter.NotifyDataSetChanged();
            //                });
            //
            return true;
        }

    }
}

