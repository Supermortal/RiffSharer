using System;

using SQLite.Net.Attributes;

namespace RiffSharer.Models
{
    public class SavedUser
    {
        [PrimaryKey]
        public int SavedUserID { get; set; }

        public string UserID { get; set; }
    }
}

