using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class Messages
    {
        public Messages()
        {
            Messagereceiver = new HashSet<Messagereceiver>();
            Messagesender = new HashSet<Messagesender>();
        }

        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Messagereceiver> Messagereceiver { get; set; }
        public virtual ICollection<Messagesender> Messagesender { get; set; }
    }
}
