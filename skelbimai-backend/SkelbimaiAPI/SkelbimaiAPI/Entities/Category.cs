using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Category
    {
        public Category()
        {
            Skelbimas = new HashSet<Skelbimas>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Skelbimas> Skelbimas { get; set; }
    }
}
