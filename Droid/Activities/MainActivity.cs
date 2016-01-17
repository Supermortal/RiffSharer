using System;
using System.Collections.Generic;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportFragment = Android.Support.V4.App.Fragment;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;

using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Concrete;
using Supermortal.Common.PCL.Abstract;

using RiffSharer.Models;
using RiffSharer.Helpers;
using RiffSharer.Repositories.Abstract;
using RiffSharer.Repositories.Concrete;
using RiffSharer.Services.Abstract;
using RiffSharer.Services.Concrete;

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
        private Stack<SupportFragment> _fragmentStack;
        private SupportFragment _currentFragment = new SupportFragment();

        #region Lifecycle

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            AddBindings();

            SetViews();
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

            IoCHelper.Instance.BindService<AUserRepository, SQLiteUserRepository>();
            IoCHelper.Instance.BindService<AAudioRepository, SQLiteAudioRepository>();
            IoCHelper.Instance.BindService<ASavedUserRepository, SQLiteSavedUserRepository>();
            IoCHelper.Instance.BindService<ISQLite, SQLite_Android>();
            IoCHelper.Instance.BindService<IRiffService, TestRiffService>();
            IoCHelper.Instance.BindService<IUserService, DefaultUserService>();
            IoCHelper.Instance.BindService<ARiffRepository, SQLiteRiffRepository>();
            IoCHelper.Instance.BindService<ACommentRepository, SQLiteCommentRepository>();
            IoCHelper.Instance.BindService<ARatingRepository, SQLiteRatingRepository>();
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

            _drawerAdapter = new ArrayAdapter<string>(this, Resource.Layout.DrawerItem, _drawerList);
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

        public void ShowFragment(Fragments fragmentEnum)
        {
            if (UserHelper.CurrentUser == null && fragmentEnum != Fragments.Login && fragmentEnum != Fragments.RegisterUser)
                return;

            var fragment = CreateFragment(fragmentEnum);

//            if (fragment.IsVisible)
//            {
//                return;
//            }

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Replace(Resource.Id.main, fragment);

            trans.Commit();

            _currentFragment = null;
            _currentFragment = fragment;
        }

        private SupportFragment CreateFragment(Fragments fragmentEnum)
        {
            SupportFragment f = null;

            switch (fragmentEnum)
            {
                case Fragments.Home:
                    f = new HomeFragment();
                    break;
                case Fragments.Login:
                    f = new LoginFragment();
                    break;
                case Fragments.Profile:
                    f = new ProfileFragment();
                    break;
                case Fragments.RecordAudio:
                    f = new RecordAudioFragment();
                    break;
                case Fragments.RegisterUser:
                    f = new RegisterUserFragment();
                    break;
                case Fragments.Riff:
                    f = new RiffFragment();
                    break;
            }

            return f;
        }

        private void SetUpFragments()
        {
            Android.Support.V4.App.FragmentTransaction tx = SupportFragmentManager.BeginTransaction();

            if (UserHelper.CurrentUser == null)
            {
                tx.Add(Resource.Id.main, new LoginFragment());
            }
            else
            {
                tx.Add(Resource.Id.main, new HomeFragment());
            }

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
                case 3:
                    ShowFragment(Fragments.RegisterUser);
                    break;
                case 4:
                    ShowFragment(Fragments.Login);
                    break;
                case 5:
                    UserHelper.Logout();
                    ShowFragment(Fragments.Login);
                    break;
                case 6:
                    ShowFragment(Fragments.Riff);
                    break;
            }

            _drawerLayout.CloseDrawers();
            _drawerToggle.SyncState();

        }

        #endregion
    }
}

