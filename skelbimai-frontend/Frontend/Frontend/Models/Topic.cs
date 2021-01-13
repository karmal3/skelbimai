using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public int ViewCounter { get; set; }
        public int FkUserId { get; set; }
        public int FkForumcategoryId { get; set; }
        public int CommentsCount { get; set; }
    }
}
