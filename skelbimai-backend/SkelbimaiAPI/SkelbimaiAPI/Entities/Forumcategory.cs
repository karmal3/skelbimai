using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Forumcategory
    {
        public Forumcategory()
        {
            Topic = new HashSet<Topic>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ViewCounter { get; set; }
        public int FkUserId { get; set; }

        public virtual User FkUser { get; set; }
        public virtual ICollection<Topic> Topic { get; set; }
    }
}
