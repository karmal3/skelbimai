using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkelbimaiAPI.Entities
{
    public class SenderHelper
    {
        public string Username { get; set; }
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int MessageId { get; set; }
        public string MessageText { get; set; }
    }
}
