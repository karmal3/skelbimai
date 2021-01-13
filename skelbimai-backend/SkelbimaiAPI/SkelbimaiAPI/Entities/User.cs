using System;
using System.Collections.Generic;

namespace SkelbimaiAPI.Entities
{
    public partial class User
    {
        public User()
        {
            BlocksFkAdmin = new HashSet<Blocks>();
            Comments = new HashSet<Comments>();
            Commentsrating = new HashSet<Commentsrating>();
            Forumcategory = new HashSet<Forumcategory>();
            Forumcomments = new HashSet<Forumcomments>();
            Forumcommentsrating = new HashSet<Forumcommentsrating>();
            MessagereceiverReceiver = new HashSet<Messagereceiver>();
            MessagereceiverSender = new HashSet<Messagereceiver>();
            MessagesenderReceiver = new HashSet<Messagesender>();
            MessagesenderSender = new HashSet<Messagesender>();
            Skelbimas = new HashSet<Skelbimas>();
            Skelbimasrating = new HashSet<Skelbimasrating>();
            Topic = new HashSet<Topic>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int FkCountryId { get; set; }
        public int Role { get; set; }
        public DateTime Date { get; set; }
        public string ProfilePicture { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }

        public virtual Country FkCountry { get; set; }
        public virtual Roles RoleNavigation { get; set; }
        public virtual Blocks BlocksFkUser { get; set; }
        public virtual ICollection<Blocks> BlocksFkAdmin { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Commentsrating> Commentsrating { get; set; }
        public virtual ICollection<Forumcategory> Forumcategory { get; set; }
        public virtual ICollection<Forumcomments> Forumcomments { get; set; }
        public virtual ICollection<Forumcommentsrating> Forumcommentsrating { get; set; }
        public virtual ICollection<Messagereceiver> MessagereceiverReceiver { get; set; }
        public virtual ICollection<Messagereceiver> MessagereceiverSender { get; set; }
        public virtual ICollection<Messagesender> MessagesenderReceiver { get; set; }
        public virtual ICollection<Messagesender> MessagesenderSender { get; set; }
        public virtual ICollection<Skelbimas> Skelbimas { get; set; }
        public virtual ICollection<Skelbimasrating> Skelbimasrating { get; set; }
        public virtual ICollection<Topic> Topic { get; set; }
    }
}
