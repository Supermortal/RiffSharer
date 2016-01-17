using System;

namespace RiffSharer.Models
{
    public class RiffDTO
    {

        public string RiffID { get; set; }

        public string AudioID { get; set; }

        public string UserID { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public Audio Audio { get; set; }

        public int DurationSeconds { get; set; }

        public RiffDTO()
        {
        }

        public RiffDTO(Riff r, Audio a)
            : this(r)
        {
            Audio = a;
        }

        public RiffDTO(Riff r)
        {
            RiffID = r.RiffID;
            AudioID = r.AudioID;
            UserID = r.UserID;
            Name = r.Name;
            DateCreated = r.DateCreated;
            DurationSeconds = r.DurationSeconds;
        }
    }
}

