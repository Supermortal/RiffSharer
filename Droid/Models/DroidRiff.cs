using System;

using Supermortal.Common.PCL.Enums;

using RiffSharer.Models;

namespace RiffSharer.Droid.Models
{
    public class DroidRiff : Java.Lang.Object
    {
        public string RiffID { get; set; }

        public string AudioID { get; set; }

        public string UserID { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public Audio Audio { get; set; }

        public int DurationSeconds { get; set; }

        public DroidRiff(Riff a)
        {
            UpdateModel(a);
        }

        public DroidRiff(RiffDTO r)
        {
            UpdateModel(r);
        }

        #region Methods

        public void UpdateModel(Riff a)
        {
            Name = a.Name;
            DateCreated = a.DateCreated;
            AudioID = a.AudioID;
            UserID = a.UserID;
            RiffID = a.RiffID;
            DurationSeconds = a.DurationSeconds;
        }

        public void UpdateModel(RiffDTO a)
        {
            Name = a.Name;
            DateCreated = a.DateCreated;
            AudioID = a.AudioID;
            UserID = a.UserID;
            RiffID = a.RiffID;
            Audio = a.Audio;
            DurationSeconds = a.DurationSeconds;
        }

        #endregion
    }
}

