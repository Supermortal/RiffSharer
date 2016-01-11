
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

using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Exceptions;

using RiffSharer.Services.Abstract;
using RiffSharer.Models;
using RiffSharer.Helpers;

namespace RiffSharer.Droid
{
    public class LoginFragment : Android.Support.V4.App.Fragment
    {

        //        private readonly IUserService _us;

        private EditText _email;
        private EditText _password;
        private Button _submit;
        private CheckBox _rememberMe;

        public LoginFragment()
        {
            
        }

        #region Lifecycle

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.LoginFragment, container, false);
        }

        public override void OnResume()
        {
            base.OnResume();

            SetViews();
            SetHandlers();
        }

        #endregion

        #region Helpers

        private bool Validate(ref string email, ref string password)
        {
            var success = true;

            _email.Error = null;
            _password.Error = null;

            email = _email.Text;
            if (string.IsNullOrWhiteSpace(email))
            {
                _email.Error = Activity.Resources.GetString(Resource.String.email_is_required);
                success = false;
            }

            password = _password.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                _password.Error = Activity.Resources.GetString(Resource.String.password_is_required);
                success = false;
            }

            return success;
        }

        private void SetViews()
        {
            _email = Activity.FindViewById<EditText>(Resource.Id.loginEmail);
            _password = Activity.FindViewById<EditText>(Resource.Id.loginPassword);
            _submit = Activity.FindViewById<Button>(Resource.Id.loginSubmit);
            _rememberMe = Activity.FindViewById<CheckBox>(Resource.Id.rememberMe);
        }

        private void SetHandlers()
        {
            _submit.Click += Click_Submit;
        }

        #endregion

        #region Events

        protected void Click_Submit(object sender, EventArgs e)
        {
            var email = string.Empty;
            var password = string.Empty;

            _email.Error = null;

            if (Validate(ref email, ref password))
            {
                try
                {
                    UserHelper.Login(email, password, _rememberMe.Checked);
                }
                catch (Exception ex)
                {
                    if (ex is UserNotFoundException || ex is PasswordIncorrectException)
                        _email.Error = Activity.Resources.GetString(Resource.String.user_name_or_password_is_incorrect);
                }
            }
        }

        #endregion

        #region Static

        public static Android.Support.V4.App.Fragment newInstance(Context context)
        {
            var busrouteFragment = new LoginFragment();
            return busrouteFragment;
        }

        #endregion
    }
}

