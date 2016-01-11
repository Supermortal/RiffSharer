using System;

using RiffSharer.Models;

namespace RiffSharer.Services.Abstract
{
    public interface IUserService
    {
        User RegisterUser(string email, string userName, string password);

        User Login(string email, string userName, string password, bool rememberMe);

        User GetSavedUser();
    }
}

