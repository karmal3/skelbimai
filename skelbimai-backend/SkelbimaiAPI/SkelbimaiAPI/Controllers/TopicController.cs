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
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly SkelbimaiDBContext _context;

        public TopicController(SkelbimaiDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("view/{id}")]
        public async Task<IActionResult> ViewCounter([FromRoute] int id)
        {
            var topic = _context.Topic.FirstOrDefault(o => o.Id == id);

            topic.ViewCounter++;

            _context.Topic.Update(topic);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("total/{id}")]
        public int TotalTopics([FromRoute] int id)
        {
            return _context.Topic.Count(o => o.FkForumcategoryId == id);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public object Get([FromRoute] int id)
        {
            return _context.Topic
                .Where(o => o.FkForumcategoryId == id)
                .Select(o => new { o.Id, o.Title, o.Description, o.ViewCounter, o.FkForumcategoryId, o.FkUserId, _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.Username }).FirstOrDefault().Username, CommentsCount = _context.Forumcomments.Count(x => x.FkTopicId == o.Id) })
                .ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("get/{id}")]
        public object GetTopic([FromRoute] int id)
        {
            return _context.Topic.Where(o => o.Id == id)
                .Select(o => new {
                    o.Id,
                    o.Title,
                    o.Description,
                    o.ViewCounter,
                    o.FkForumcategoryId,
                    o.FkUserId,
                    _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.Username }).FirstOrDefault().Username
                })
                .FirstOrDefault();
        }

        [HttpGet]
        public object GetUserTopics()
        {
            var uid = int.Parse(User.Identity.Name);
            return _context.Topic.Where(o => o.FkUserId == uid)
                .Select(o => new { o.Id, o.Title, o.Description, o.ViewCounter, o.FkForumcategoryId, o.FkUserId, _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.Username }).FirstOrDefault().Username })
                .ToList();
        }

        [HttpGet]
        [Route("usertopicsadmin/{id}")]
        public object GetUserTopicsAdmin(int id)
        {
            int uid = int.Parse(User.Identity.Name);
            var admin = _context.User.FirstOrDefault(o => o.Id == uid);

            if (admin == null)
                return null;

            return _context.Topic.Where(o => o.FkUserId == id)
                .Select(o => new { o.Id, o.Title, o.Description, o.ViewCounter, o.FkForumcategoryId, o.FkUserId, _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.Username }).FirstOrDefault().Username })
                .ToList();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Topic value)
        {
            int uid = int.Parse(User.Identity.Name);
            value.FkUserId = uid;
            _context.Topic.Add(value);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Update([FromBody] Topic topic)
        {
            var id = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == id).Any();
            var updatedTopic = await _context.Topic.FindAsync(topic.Id);

            // check if user is the owner of the topic
            if (id != updatedTopic.FkUserId)
                // check if admin is trying to update user's topic
                if (role == false)
                    return BadRequest(new { message = "Authorization to edit requested topic was not granted" });

            updatedTopic.Title = topic.Title;
            updatedTopic.Description = topic.Description;

            _context.Topic.Update(updatedTopic);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Topic was updated." });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // check if user is the owner of the topic
            var uid = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == uid).Any();
            var topic = await _context.Topic.FindAsync(id);
            if (uid != topic.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to delete requested topic was not granted" });

            //checks if topic has any comments
            var count = _context.Forumcomments.Count(o => o.FkTopicId == id);

            for (int i = 0; i < count; i++)
            {
                var comment = _context.Forumcomments.FirstOrDefault(o => o.FkTopicId == id);
                _context.Forumcomments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            var topic_delete = _context.Topic.FirstOrDefault(o => o.Id == id);

            _context.Topic.Remove(topic_delete);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Ad was deleted." });
        }
    }
}