
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

namespace RiffSharer.Droid
{
    public class RiffFragment : Android.Support.V4.App.Fragment
    {
        #region Lifecycle

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.RiffFragment, container, false);
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
            
        }

        private void SetHandlers()
        {
            
        }

        #endregion

        #region Static

        public static Android.Support.V4.App.Fragment newInstance(Context context)
        {
            var busrouteFragment = new RiffFragment();
            return busrouteFragment;
        }

        #endregion
    }
}

