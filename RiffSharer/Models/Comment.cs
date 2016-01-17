using System;

namespace RiffSharer.Models
{
    public class Comment
    {
        public string CommentID { get; set; }

        public string UserID { get; set; }

        public string RiffID { get; set; }

        public string Text { get; set; }

        public DateTime DateCreated { get; set; }

        public void UpdateModel(Comment c)
        {
            UserID = c.UserID;
            RiffID = c.RiffID;
            Text = c.Text;
        }
    }
}

