using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Commentsrating
    {
        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkCommentId { get; set; }
        public int Upvote { get; set; }
        public int Downvote { get; set; }

        public virtual Comments FkComment { get; set; }
        public virtual User FkUser { get; set; }
    }
}
