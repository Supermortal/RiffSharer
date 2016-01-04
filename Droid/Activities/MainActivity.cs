using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using RiffSharer.Models;
using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Concrete;
using Supermortal.Common.PCL.Abstract;
using RiffSharer;
using RiffSharer.Repositories.Abstract;
using RiffSharer.Repositories.Concrete;
using RiffSharer.Services.Abstract;
using RiffSharer.Services.Concrete;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace RiffSharer.Droid
{
    [Activity(Label = "RiffSharer.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Android.Support.V7.App.AppCompatActivity
    {

        private SupportToolbar _toolbar;
        private DrawerLayout _drawerLayout;
        private ListView _drawerView;
        private string[] _drawerList;
        private ArrayAdapter _drawerAdapter;
        private ActionBarDrawerToggle _drawerToggle;
        private Dictionary<Fragments, SupportFragment> _fragments = new Dictionary<Fragments, SupportFragment>();
        private Stack<SupportFragment> _fragmentStack;
        private SupportFragment _currentFragment = new SupportFragment();

        #region Lifecycle

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            AddBindings();

            SetViews();
            CreateFragments();
            SetUpFragments();
            SetUpActionBar();
            SetUpNavigationDrawer();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _drawerToggle.OnConfigurationChanged(newConfig);
        }

        #endregion

        #region Helpers

        private void AddBindings()
        {
            IoCHelper.Instance = new NinjectIoCHelper();

            IoCHelper.Instance.BindService<AAudioRepository, SQLiteAudioRepository>();
            IoCHelper.Instance.BindService<ISQLite, SQLite_Android>();
            IoCHelper.Instance.BindService<IAudioService, DefaultAudioService>();
        }

        private void SetViews()
        {
            _toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            _drawerView = FindViewById<ListView>(Resource.Id.left_drawer);
        }

        private void SetUpActionBar()
        {
            SetSupportActionBar(_toolbar);

            _toolbar.Click += (object sender, EventArgs e) =>
            {
                _drawerLayout.OpenDrawer(_drawerView);
            };

            SupportActionBar.SetTitle(Resource.String.app_name);
        }

        private void SetUpNavigationDrawer()
        {
            _drawerList = Resources.GetStringArray(Resource.Array.drawer_list);

            _drawerAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, _drawerList);
            _drawerView.Adapter = _drawerAdapter;
            _drawerView.ItemClick += MenuListView_ItemClick;


            _drawerToggle = new ActionBarDrawerToggle(
                this,                           //Host Activity
                _drawerLayout,                  //DrawerLayout
                Resource.String.app_name,     //Opened Message
                Resource.String.app_name     //Closed Message
            );

            _drawerLayout.SetDrawerListener(_drawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            _drawerToggle.SyncState();
        }

        private void ShowFragment(Fragments fragmentEnum)
        {
            var fragment = _fragments[fragmentEnum];

            if (fragment.IsVisible)
            {
                return;
            }

            var trans = SupportFragmentManager.BeginTransaction();

            fragment.View.BringToFront();
            _currentFragment.View.BringToFront();

            trans.Hide(_currentFragment);
            trans.Show(fragment);

            trans.AddToBackStack(null);
            _fragmentStack.Push(_currentFragment);
            trans.Commit();

            _currentFragment = fragment;
        }

        private void CreateFragments()
        {
            _fragments[Fragments.Home] = new HomeFragment();
            _fragments[Fragments.Profile] = new ProfileFragment();
            _fragments[Fragments.RecordAudio] = new RecordAudioFragment();
            _fragmentStack = new Stack<SupportFragment>();
        }

        private void SetUpFragments()
        {
            Android.Support.V4.App.FragmentTransaction tx = SupportFragmentManager.BeginTransaction();

            tx.Add(Resource.Id.main, _fragments[Fragments.Home]);
            tx.Add(Resource.Id.main, _fragments[Fragments.Profile]);
            tx.Add(Resource.Id.main, _fragments[Fragments.RecordAudio]);

            tx.Hide(_fragments[Fragments.Profile]);
            tx.Hide(_fragments[Fragments.RecordAudio]);

            _currentFragment = _fragments[Fragments.Home];

            tx.Commit();
        }

        #endregion

        #region Menu

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ActionMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            { 
                case Android.Resource.Id.Home:
                    //The hamburger icon was clicked which means the drawer toggle will handle the event
                    _drawerToggle.OnOptionsItemSelected(item);
                    return true;
                case Resource.Id.action_refresh:
                    //Refresh
                    return true;
                case Resource.Id.action_help:
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        #endregion

        #region Events

        private void MenuListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (e.Id)
            {
                case 0:
                    ShowFragment(Fragments.Home);
                    break;
                case 1:
                    ShowFragment(Fragments.Profile);
                    break;
                case 2:
                    ShowFragment(Fragments.RecordAudio);
                    break;
            }

            //SupportFragmentManager.BeginTransaction().Replace(Resource.Id.main, fragment).Commit();

            _drawerLayout.CloseDrawers();
            _drawerToggle.SyncState();

        }

        #endregion
    }
}

