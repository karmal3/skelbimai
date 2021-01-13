using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkelbimaiAPI.Entities
{
    public class Graph
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int GraphType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
