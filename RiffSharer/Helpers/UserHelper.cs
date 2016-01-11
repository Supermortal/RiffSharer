using System;

using Supermortal.Common.PCL.Helpers;

using RiffSharer.Models;
using RiffSharer.Services.Abstract;

namespace RiffSharer.Helpers
{
    public class UserHelper
    {

        private static IUserService _userService;

        private static IUserService UserService
        { 
            get
            {
                return _userService ?? (_userService = IoCHelper.Instance.GetService<IUserService>());
            } 
        }

        private static bool _currentUserAccessedFirstTime = false;
        private static User _currentUser;

        public static User CurrentUser
        { 
            get
            {
                if (!_currentUserAccessedFirstTime && _currentUser == null)
                {
                    _currentUserAccessedFirstTime = true;
                    _currentUser = GetSavedUser();
                }

                return _currentUser;
            } 
            private set
            {
                _currentUser = value;
            }
        }

        #region Methods

        public static void RegisterUser(string email, string userName, string password, bool setAsCurrentUser = false)
        {
            var user = UserService.RegisterUser(email, userName, password);

            if (setAsCurrentUser)
                CurrentUser = user;
        }

        public static void Login(string email, string password, bool rememberMe = false)
        {
            CurrentUser = UserService.Login(email, null, password, rememberMe);
        }

        private static User GetSavedUser()
        {
            return UserService.GetSavedUser();
        }

        #endregion
    }
}

