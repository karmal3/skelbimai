using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkSkelbimasId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public int LikeCounter { get; set; }
        public int DislikeCounter { get; set; }
        public string ProfilePicture { get; set; }
        public bool Liked { get; set; }
        public bool Disliked { get; set; }
    }
}
