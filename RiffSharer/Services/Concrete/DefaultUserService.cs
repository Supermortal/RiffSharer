using System;

using Supermortal.Common.PCL.Helpers;
using Supermortal.Common.PCL.Exceptions;

using RiffSharer.Repositories.Abstract;
using RiffSharer.Services.Abstract;

namespace RiffSharer.Services.Concrete
{
    public class DefaultUserService : IUserService
    {

        private readonly AUserRepository _ur;

        public DefaultUserService()
            : this(IoCHelper.Instance.GetService<AUserRepository>())
        {
        }

        public DefaultUserService(AUserRepository ur)
        {
            _ur = ur;
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

        public User Login(string email, string userName, string password)
        {
            email = email.Trim();
            userName = userName.Trim();
            User user = null;

            if (!string.IsNullOrWhiteSpace(email))
                user = _ur.FindByEmail(email);

            if (user == null && !string.IsNullOrWhiteSpace(userName))
                user = _ur.FindByUsername(userName);

            if (user == null)
                throw new UserNotFoundException();

            if (user.Password != Crypto.Hash(password))
                throw new PasswordIncorrectException();

            return user;
        }
    }
}

