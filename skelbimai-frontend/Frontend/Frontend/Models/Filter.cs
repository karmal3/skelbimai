using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Filter
    {
        public List<int> Id { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int Date { get; set; }
        public int Rating { get; set; }
        public int PriceOrder { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int GraphType { get; set; }
    }
}
