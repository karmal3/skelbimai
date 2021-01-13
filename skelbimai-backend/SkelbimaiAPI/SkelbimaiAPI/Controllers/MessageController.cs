using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkelbimaiAPI.Entities;

namespace SkelbimaiAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly SkelbimaiDBContext _context;

        public MessageController(SkelbimaiDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("inboxcount")]
        public int InboxCount()
        {
            var uid = int.Parse(User.Identity.Name);

            return _context.Messagereceiver.Count(o => o.ReceiverId == uid);
        }

        [HttpGet]
        [Route("sentcount")]
        public int SentCount()
        {
            var uid = int.Parse(User.Identity.Name);

            return _context.Messagesender.Count(o => o.SenderId == uid);
        }


        [HttpGet]
        [Route("inbox")]
        public object getReceivedMessages()
        {
            var uid = int.Parse(User.Identity.Name);

            return _context.Messagereceiver
                .Where(o => o.ReceiverId == uid)
                .Select(o => new { o.Id, o.MessageId, o.Message.Message, o.SenderId, Sender = o.Sender.Username, o.Message.Date })
                .OrderByDescending(o => o.Date)
                .ToList();
        }

        [HttpGet]
        [Route("sent")]
        public object getSentMessages()
        {
            var uid = int.Parse(User.Identity.Name);

            return _context.Messagesender
                .Where(o => o.SenderId == uid)
                .Select(o => new { o.Id, o.MessageId, o.Message.Message, o.ReceiverId, Receiver = o.Receiver.Username, o.Message.Date })
                .OrderByDescending(o => o.Date)
                .ToList();
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> Send([FromBody] SenderHelper sender)
        {
            var RECEIVER = _context.User.FirstOrDefault(o => o.Username == sender.Username.ToLower());

            if (RECEIVER == null)
                return BadRequest(new { message = "User with given username was not found." });

            Messages newMessage = new Messages();
            newMessage.Message = sender.MessageText;
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            try
            {
                Messagesender Sender = new Messagesender();
                Sender.MessageId = newMessage.Id;
                Sender.SenderId = int.Parse(User.Identity.Name);
                Sender.ReceiverId = RECEIVER.Id;
                _context.Messagesender.Add(Sender);
                await _context.SaveChangesAsync();

                Messagereceiver receiver = new Messagereceiver();
                receiver.MessageId = newMessage.Id;
                receiver.SenderId = int.Parse(User.Identity.Name);
                receiver.ReceiverId = RECEIVER.Id;
                _context.Messagereceiver.Add(receiver);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _context.Messages.Remove(newMessage);
                return BadRequest(new { message = "Message was not sent. " + e.Message });
            }

            return Ok(new { message = "Message sent." });
        }

        [HttpDelete]
        [Route("inboxdelete/{id}")]
        public async Task<IActionResult> DeleteInboxMessage([FromRoute] int id)
        {
            var uid = int.Parse(User.Identity.Name);

            var InboxMessage = _context.Messagereceiver.FirstOrDefault(o => o.Id == id && o.ReceiverId == uid);

            if (InboxMessage == null)
                return BadRequest(new { message = "Message was not deleted!" });

            _context.Messagereceiver.Remove(InboxMessage);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Message was deleted." });
        }

        [HttpDelete]
        [Route("sentdelete/{id}")]
        public async Task<IActionResult> DeleteSentMessage([FromRoute] int id)
        {
            var uid = int.Parse(User.Identity.Name);

            var SentMessage = _context.Messagesender.FirstOrDefault(o => o.Id == id && o.SenderId == uid);

            if (SentMessage == null)
                return BadRequest(new { message = "Message was not deleted!" });

            _context.Messagesender.Remove(SentMessage);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Message was deleted." });
        }
    }
}