using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Frontend.Models
{
    public class Blocks
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int FkUserId { get; set; }
        public int FkAdminId { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}
