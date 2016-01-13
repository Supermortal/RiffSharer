using System;
using System.Threading.Tasks;

namespace RiffSharer.Droid.Adapters
{
    public class AudioScrollListener : EndlessScrollListener
    {

        //        private IServerWebService _sws;
        //        private LeadsListAdapter _adapter;
        //        private Activity _activity;
        //
        //        public LeadsScrollListener(IServerWebService sws, LeadsListAdapter adapter, Activity activity, LinearLayoutManager manager)
        //            : base(manager)
        //        {
        //            _sws = sws;
        //            _adapter = adapter;
        //            _activity = activity;
        //        }
        //
        public async override Task<bool> OnLoadMore(int page, int totalItemsCount)
        {
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

