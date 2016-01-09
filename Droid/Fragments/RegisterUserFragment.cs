
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

namespace RiffSharer.Droid
{
    public class RegisterUserFragment : Android.Support.V4.App.Fragment
    {

        private readonly IUserService _us;

        private EditText _email;
        private EditText _confirmEmail;
        private EditText _userName;
        private EditText _password;
        private EditText _confirmPassword;
        private Button _submit;

        public RegisterUserFragment()
            : this(IoCHelper.Instance.GetService<IUserService>())
        {
            
        }

        public RegisterUserFragment(IUserService us)
        {
            _us = us;
        }

        #region Lifecycle

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.RegisterUserFragment, container, false);
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
            _email = Activity.FindViewById<EditText>(Resource.Id.email);
            _email.RequestFocus();

            _confirmEmail = Activity.FindViewById<EditText>(Resource.Id.confirmEmail);
            _userName = Activity.FindViewById<EditText>(Resource.Id.userName);
            _password = Activity.FindViewById<EditText>(Resource.Id.password);
            _confirmPassword = Activity.FindViewById<EditText>(Resource.Id.confirmPassword);
            _submit = Activity.FindViewById<Button>(Resource.Id.submit);
        }

        private void SetHandlers()
        {
            _submit.Click += Click_Submit;
        }

        private bool Validate(ref string email, ref string userName, ref string password)
        {
            var success = true;

            _email.Error = null;
            _confirmEmail.Error = null;
            _userName.Error = null;
            _password.Error = null;
            _confirmPassword.Error = null;

            email = _email.Text;
            if (string.IsNullOrWhiteSpace(email))
            {
                _email.Error = Activity.Resources.GetString(Resource.String.email_is_required);
                success = false;
            }

            var regex = RegexHelper.GetEmailRegex();
            if (!regex.IsMatch(email))
            {
                _email.Error = Activity.Resources.GetString(Resource.String.email_must_be_valid);
                success = false;
            }

            var confirmEmail = _confirmEmail.Text;
            if (confirmEmail != email)
            {
                _confirmEmail.Error = Activity.Resources.GetString(Resource.String.email_does_not_match);
                success = false;
            }

            userName = _userName.Text;
            if (string.IsNullOrWhiteSpace(userName))
            {
                _userName.Error = Activity.Resources.GetString(Resource.String.user_name_is_required);
                success = false;
            }

            password = _password.Text;
            if (string.IsNullOrWhiteSpace(password))
            {
                _password.Error = Activity.Resources.GetString(Resource.String.password_is_required);
                success = false;
            }

            regex = RegexHelper.GetPasswordRegex();
            if (!regex.IsMatch(password))
            {
                _password.Error = Activity.Resources.GetString(Resource.String.strong_password_is_required);
                success = false;
            }

            var confirmPassword = _confirmPassword.Text;
            if (confirmPassword != password)
            {
                _confirmPassword.Error = Activity.Resources.GetString(Resource.String.password_does_not_match);
                success = false;
            }

            return success;
        }

        #endregion

        #region Events

        protected void Click_Submit(object sender, EventArgs e)
        {
            var email = string.Empty;
            var userName = string.Empty;
            var password = string.Empty;

            if (Validate(ref email, ref userName, ref password))
            {
                User user = null;
                _email.Error = null;
                _userName.Error = null;

                try
                {
                    user = _us.RegisterUser(email, userName, password);
                    var u = _us.Login(email, userName, password);
                }
                catch (EmailAlreadyInUseException ex)
                {
                    _email.Error = Activity.Resources.GetString(Resource.String.email_already_in_use);
                }
                catch (UserNameAlreadyInUseException ex)
                {
                    _userName.Error = Activity.Resources.GetString(Resource.String.user_name_already_in_use);
                }
            }
        }

        #endregion

        #region Static

        public static Android.Support.V4.App.Fragment newInstance(Context context)
        {
            var busrouteFragment = new RegisterUserFragment();
            return busrouteFragment;
        }

        #endregion
    }
}

