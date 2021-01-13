using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Skelbimas
    {
        public Skelbimas()
        {
            Comments = new HashSet<Comments>();
            Skelbimasrating = new HashSet<Skelbimasrating>();
        }

        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkCategoryId { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public int ViewCounter { get; set; }

        public virtual Category FkCategory { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Skelbimasrating> Skelbimasrating { get; set; }
    }
}
