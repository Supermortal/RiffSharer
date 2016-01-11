using System;

using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Exceptions;

using RiffSharer.Repositories.Abstract;
using RiffSharer.Services.Abstract;
using RiffSharer.Models;

namespace RiffSharer.Services.Concrete
{
    public class DefaultUserService : IUserService
    {

        private readonly AUserRepository _ur;
        private readonly ASavedUserRepository _sur;

        public DefaultUserService()
            : this(IoCHelper.Instance.GetService<AUserRepository>(), IoCHelper.Instance.GetService<ASavedUserRepository>())
        {
        }

        public DefaultUserService(AUserRepository ur, ASavedUserRepository sur)
        {
            _ur = ur;
            _sur = sur;
        }

        public User RegisterUser(string email, string userName, string password)
        {
            email = email.Trim();
            userName = userName.Trim();

            if (_ur.CheckEmail(email))
                throw new EmailAlreadyInUseException();

            if (_ur.CheckUsername(userName))
                throw new UserNameAlreadyInUseException();

            var user = new User();

            user.UserName = userName;
            user.Email = email;
            user.Password = Crypto.Hash(password);

            return _ur.Insert(user);
        }

        public User Login(string email, string userName, string password, bool rememberMe)
        {
            if (!string.IsNullOrWhiteSpace(email))
                email = email.Trim();
            if (!string.IsNullOrWhiteSpace(userName))
                userName = userName.Trim();
            
            User user = null;

            if (!string.IsNullOrWhiteSpace(email))
                user = _ur.FindByEmail(email);

            if (user == null && !string.IsNullOrEmpty(userName))
                user = _ur.FindByUsername(userName);

            if (user == null)
                throw new UserNotFoundException();

            if (user.Password != Crypto.Hash(password))
                throw new PasswordIncorrectException();

            if (rememberMe)
                _sur.InsertOrUpdate(new SavedUser() { UserID = user.UserID });

            return user;
        }

        public User GetSavedUser()
        {
            var su = _sur.Get();

            if (su == null)
                return null;

            return _ur.Get(su.UserID);
        }
    }
}

