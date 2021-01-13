using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Skelbimasrating
    {
        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkSkelbimasId { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }

        public virtual Skelbimas FkSkelbimas { get; set; }
        public virtual User FkUser { get; set; }
    }
}
