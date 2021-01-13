using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Blocks
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int FkUserId { get; set; }
        public int FkAdminId { get; set; }
        public DateTime Date { get; set; }

        public virtual User FkAdmin { get; set; }
        public virtual User FkUser { get; set; }
    }
}
