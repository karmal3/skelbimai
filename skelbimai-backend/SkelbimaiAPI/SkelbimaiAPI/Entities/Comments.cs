using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Comments
    {
        public Comments()
        {
            Commentsrating = new HashSet<Commentsrating>();
        }

        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkSkelbimasId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int LikeCounter { get; set; }
        public int DislikeCounter { get; set; }

        public virtual Skelbimas FkSkelbimas { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<Commentsrating> Commentsrating { get; set; }
    }
}
