using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class ForumCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public int ViewCounter { get; set; }
        public int FkUserId { get; set; }
        public int Total { get; set; }
    }
}
