
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
    class HomeFragment : Android.Support.V4.App.Fragment
    {
        public HomeFragment()
        {

        }

        public static Android.Support.V4.App.Fragment newInstance(Context context)
        {
            HomeFragment busrouteFragment = new HomeFragment();
            return busrouteFragment;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            ViewGroup root = (ViewGroup)inflater.Inflate(Resource.Layout.HomeFragment, null);
            return root;
        }
    }
}

