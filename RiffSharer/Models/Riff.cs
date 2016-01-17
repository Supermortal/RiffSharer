using System;

using SQLite.Net.Attributes;

namespace RiffSharer.Models
{
    public class Riff
    {
        [PrimaryKey]
        public string RiffID { get; set; }

        public string AudioID { get; set; }

        public string UserID { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public void UpdateModel(Riff r)
        {
            AudioID = r.AudioID;
            Name = r.Name;
            UserID = r.UserID;
        }
    }
}

