using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Messagesender
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int MessageId { get; set; }

        public virtual Messages Message { get; set; }
        public virtual User Receiver { get; set; }
        public virtual User Sender { get; set; }
    }
}
