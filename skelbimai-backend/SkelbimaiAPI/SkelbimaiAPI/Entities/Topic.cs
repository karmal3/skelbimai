using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Topic
    {
        public Topic()
        {
            Forumcomments = new HashSet<Forumcomments>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int ViewCounter { get; set; }
        public int FkUserId { get; set; }
        public int FkForumcategoryId { get; set; }

        public virtual Forumcategory FkForumcategory { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<Forumcomments> Forumcomments { get; set; }
    }
}
