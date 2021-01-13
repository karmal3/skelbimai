using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Forumcomments
    {
        public Forumcomments()
        {
            Forumcommentsrating = new HashSet<Forumcommentsrating>();
        }

        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkTopicId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int LikeCounter { get; set; }
        public int DislikeCounter { get; set; }

        public virtual Topic FkTopic { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<Forumcommentsrating> Forumcommentsrating { get; set; }
    }
}
