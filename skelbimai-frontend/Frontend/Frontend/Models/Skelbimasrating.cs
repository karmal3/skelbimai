using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Skelbimasrating
    {
        public int Id { get; set; }
        public int FkUserId { get; set; }
        public int FkSkelbimasId { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
