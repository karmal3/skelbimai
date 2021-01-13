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
    public class ForumController : ControllerBase
    {
        private readonly SkelbimaiDBContext _context;

        public ForumController(SkelbimaiDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("view/{id}")]
        public async Task<IActionResult> ViewCounter([FromRoute] int id)
        {
            var category = _context.Forumcategory.FirstOrDefault(o => o.Id == id);

            category.ViewCounter++;

            _context.Forumcategory.Update(category);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public object Get()
        {
            return _context.Forumcategory.Select(o => new { o.Id, o.Name, o.Description, o.ViewCounter, o.FkUserId, _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.Username }).FirstOrDefault().Username, Total = _context.Topic.Count(g => g.FkForumcategoryId == o.Id) }).ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("total")]
        public int TotalPosts()
        {
            return _context.Topic.Count();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Forumcategory value)
        {
            int uid = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == uid).Any();

            if (role == false)
                return BadRequest(new { message = "Authorization to create new forum category was not granted" });

            value.FkUserId = uid;
            _context.Forumcategory.Add(value);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Update([FromBody] Forumcategory forumCategory)
        {
            var id = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == id).Any();
            var updatedForumCategory = await _context.Forumcategory.FindAsync(forumCategory.Id);

            // check if user is the owner of the forum category
            if (id != updatedForumCategory.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to edit requested forum category was not granted." });

            updatedForumCategory.Name = forumCategory.Name;
            updatedForumCategory.Description = forumCategory.Description;

            _context.Forumcategory.Update(updatedForumCategory);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Forum category was updated." });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // check if user is the owner of the forum category
            var uid = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == uid).Any();
            var forumCategory = await _context.Forumcategory.FindAsync(id);

            if (uid != forumCategory.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to delete requested forum category was not granted." });

            // checks if forum category has any topics
            var count = _context.Topic.Count(o => o.FkForumcategoryId == id);

            for (int i = 0; i < count; i++)
            {
                var topic = _context.Topic.FirstOrDefault(o => o.FkForumcategoryId == id);
                _context.Topic.Remove(topic);
                await _context.SaveChangesAsync();
            }

            var forumCategory_delete = _context.Forumcategory.FirstOrDefault(o => o.Id == id);

            _context.Forumcategory.Remove(forumCategory_delete);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Forum category was deleted." });
        }
    }
}
