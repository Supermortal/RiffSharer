using System;

namespace RiffSharer.Models
{
    public class Audio
    {
        public string AudioID { get; set; }

        public string Name { get; set; }

        public string LocalPath { get; set; }

        public DateTime DateCreated { get; set; }

        public void UpdateModel(Audio a)
        {
            LocalPath = a.LocalPath;
            Name = a.Name;
        }
    }
}

