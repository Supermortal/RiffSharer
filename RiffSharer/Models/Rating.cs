using System;

namespace RiffSharer.Models
{
    public class Rating
    {
        public string RatingID { get; set; }

        public string UserID { get; set; }

        public string RiffID { get; set; }

        public float Value { get; set; }

        public DateTime DateCreated { get; set; }

        public void UpdateModel(Rating r)
        {
            UserID = r.UserID;
            RiffID = r.RiffID;
            Value = r.Value;
        }
    }
}

