using System;

using SQLite.Net.Attributes;

namespace RiffSharer.Models
{
    public class User
    {
        [PrimaryKey]
        public string UserID { get; set; }

        public string UserName { get; set; }

        public DateTime DateCreated { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public void UpdateModel(User u)
        {
            UserName = u.UserName;
            Password = u.Password;
        }
    }
}

