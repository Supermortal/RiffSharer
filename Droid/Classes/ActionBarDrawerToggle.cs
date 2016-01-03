using System;
using SupportActionBarDrawerToggle = Android.Support.V7.App.ActionBarDrawerToggle;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace RiffSharer
{
    public class ActionBarDrawerToggle : SupportActionBarDrawerToggle
    {
        private ActionBarActivity _activity;
        private int _openedResource;
        private int _closedResource;

        public ActionBarDrawerToggle(ActionBarActivity host, DrawerLayout drawerLayout, int openedResource, int closedResource)
            : base(host, drawerLayout, openedResource, closedResource)
        {
            _activity = host;
            _openedResource = openedResource;
            _closedResource = closedResource;
        }

        public override void OnDrawerOpened(Android.Views.View drawerView)
        {   
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                base.OnDrawerOpened(drawerView);
                _activity.SupportActionBar.SetTitle(_openedResource);
            }
        }

        public override void OnDrawerClosed(Android.Views.View drawerView)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                base.OnDrawerClosed(drawerView);
                _activity.SupportActionBar.SetTitle(_closedResource);
            }               
        }

        public override void OnDrawerSlide(Android.Views.View drawerView, float slideOffset)
        {
            int drawerType = (int)drawerView.Tag;

            if (drawerType == 0)
            {
                base.OnDrawerSlide(drawerView, slideOffset);
            }
        }
    }
}

