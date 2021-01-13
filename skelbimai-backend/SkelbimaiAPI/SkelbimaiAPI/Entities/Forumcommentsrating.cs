using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Forumcommentsrating
    {
        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkForumCommentId { get; set; }
        public int Upvote { get; set; }
        public int Downvote { get; set; }

        public virtual Forumcomments FkForumComment { get; set; }
        public virtual User FkUser { get; set; }
    }
}
