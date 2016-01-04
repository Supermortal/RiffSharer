using System;

using SQLite.Net.Attributes;

namespace RiffSharer
{
    public class User
    {
        [PrimaryKey]
        public string UserID { get; set; }

        public string UserName { get; set; }

        public DateTime DateCreated { get; set; }

        public void UpdateModel(User u)
        {
            UserName = u.UserName;
        }
    }
}

