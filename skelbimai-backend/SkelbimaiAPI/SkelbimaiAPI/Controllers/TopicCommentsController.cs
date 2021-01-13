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
    public class TopicCommentsController : ControllerBase
    {
        private readonly SkelbimaiDBContext _context;

        public TopicCommentsController(SkelbimaiDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public object GetTopicComments([FromRoute] int id)
        {
            try
            {
                var uid = int.Parse(User.Identity.Name);

                return _context.Forumcomments.Where(o => o.FkTopicId == id)
                .Select(o => new
                {
                    o.Id,
                    o.FkUserId,
                    o.LikeCounter,
                    o.Description,
                    o.Date,
                    o.FkTopicId,
                    _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.Username }).FirstOrDefault().Username,
                    o.DislikeCounter,
                    _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.ProfilePicture }).FirstOrDefault().ProfilePicture,
                    Disliked = _context.Forumcommentsrating.Where(x => x.FkForumCommentId == o.Id && x.FkUserId == uid && x.Downvote == 1).Any(),
                    Liked = _context.Forumcommentsrating.Where(x => x.FkForumCommentId == o.Id && x.FkUserId == uid && x.Upvote == 1).Any()
                })
                .OrderByDescending(o => o.Date)
                .ToList();
            }
            catch
            {
                return _context.Forumcomments.Where(o => o.FkTopicId == id)
                   .Select(o => new {
                       o.Id,
                       o.FkUserId,
                       o.LikeCounter,
                       o.Description,
                       o.Date,
                       o.FkTopicId,
                       _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.Username }).FirstOrDefault().Username,
                       o.DislikeCounter,
                       _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.ProfilePicture }).FirstOrDefault().ProfilePicture
                   })
                   .OrderByDescending(o => o.Date)
                   .ToList();
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] Forumcomments value)
        {
            value.FkUserId = int.Parse(User.Identity.Name);
            value.Date = DateTime.Now;
            _context.Forumcomments.Add(value);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Update([FromBody] Forumcomments comment)
        {
            // check if user is the owner of question's quiz
            var uid = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == uid).Any();
            var updatedComment = await _context.Forumcomments.FindAsync(comment.Id);
            if (uid != updatedComment.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to update requested topic comment was not granted" });

            updatedComment.Description = comment.Description;
            updatedComment.Date = DateTime.Now;

            _context.Forumcomments.Update(updatedComment);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Comment updated." });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // check if user is the owner of question's quiz
            var uid = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == uid).Any();
            var forumComment = await _context.Forumcomments.FindAsync(id);
            if (uid != forumComment.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to delete requested comment not granted" });

            // checks if comment has any ratings
            var count = _context.Forumcommentsrating.Count(o => o.FkForumCommentId == forumComment.Id);

            for (int i = 0; i < count; i++)
            {
                var rating = _context.Forumcommentsrating.FirstOrDefault(o => o.FkForumCommentId == forumComment.Id);
                _context.Forumcommentsrating.Remove(rating);
                await _context.SaveChangesAsync();
            }

            var comment_delete = _context.Forumcomments.FirstOrDefault(o => o.Id == id);

            _context.Forumcomments.Remove(comment_delete);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("up/{id}")]
        public async Task<IActionResult> UpVote([FromRoute] int id)
        {
            var uid = int.Parse(User.Identity.Name);
            var comment = await _context.Forumcomments.FindAsync(id);

            if (comment == null)
                return BadRequest(new { message = "Comment was not found" });

            // check if user already upvoted or downvoted the comment
            if (_context.Forumcommentsrating.Where(o => o.FkUserId == uid && o.FkForumCommentId == comment.Id).Any())
            {
                // user already upvoted or downvoted
                var rating = _context.Forumcommentsrating.Where(o => o.FkUserId == uid && o.FkForumCommentId == comment.Id).FirstOrDefault();
                if (rating.Upvote == 1)
                {
                    // if comment is already upvoted then delete the rating
                    _context.Forumcommentsrating.Remove(rating);
                    await _context.SaveChangesAsync();
                    var likeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Upvote == 1).Count();
                    comment.DislikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Downvote == 1).Count();
                    comment.LikeCounter = likeCounter;
                    _context.Forumcomments.Update(comment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Rating was removed." });
                }
                else
                {
                    // user hasn't upvoted or has already downvoted the comment

                    rating.Upvote = 1;
                    rating.Downvote = 0;
                    _context.Forumcommentsrating.Update(rating);
                    await _context.SaveChangesAsync();
                    comment.LikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Upvote == 1).Count();
                    comment.DislikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Downvote == 1).Count();
                    _context.Forumcomments.Update(comment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Rating was upvoted." });
                }
            }
            else
            {
                // user hasn't upvoted or downvoted the comment
                Forumcommentsrating rating = new Forumcommentsrating();
                rating.FkUserId = uid;
                rating.FkForumCommentId = comment.Id;
                rating.Upvote = 1;
                _context.Forumcommentsrating.Add(rating);
                await _context.SaveChangesAsync();
                comment.LikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Upvote == 1).Count();
                comment.DislikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Downvote == 1).Count();
                _context.Forumcomments.Update(comment);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Rating was created and upvoted." });
            }
        }

        [HttpPost]
        [Route("down/{id}")]
        public async Task<IActionResult> DownVote([FromRoute] int id)
        {
            var uid = int.Parse(User.Identity.Name);
            var comment = await _context.Forumcomments.FindAsync(id);

            if (comment == null)
                return BadRequest(new { message = "Comment was not found" });

            // check if user already upvoted or downvoted the comment
            if (_context.Forumcommentsrating.Where(o => o.FkUserId == uid && o.FkForumCommentId == comment.Id).Any())
            {
                // user already upvoted or downvoted
                var rating = _context.Forumcommentsrating.Where(o => o.FkUserId == uid && o.FkForumCommentId == comment.Id).FirstOrDefault();
                if (rating.Downvote == 1)
                {
                    // if comment is already downvoted then delete the rating
                    _context.Forumcommentsrating.Remove(rating);
                    await _context.SaveChangesAsync();
                    comment.LikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Upvote == 1).Count();
                    comment.DislikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Downvote == 1).Count();
                    _context.Forumcomments.Update(comment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Rating was removed." });
                }
                else
                {
                    // user hasn't downvoted or has already upvoted the comment

                    rating.Upvote = 0;
                    rating.Downvote = 1;
                    _context.Forumcommentsrating.Update(rating);
                    await _context.SaveChangesAsync();
                    comment.LikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Upvote == 1).Count();
                    comment.DislikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Downvote == 1).Count();
                    _context.Forumcomments.Update(comment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Rating was downvoted." });
                }
            }
            else
            {
                // user hasn't upvoted or downvoted the comment
                Forumcommentsrating rating = new Forumcommentsrating();
                rating.FkUserId = uid;
                rating.FkForumCommentId = comment.Id;
                rating.Downvote = 1;
                _context.Forumcommentsrating.Add(rating);
                await _context.SaveChangesAsync();
                comment.LikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Upvote == 1).Count();
                comment.DislikeCounter = _context.Forumcommentsrating.Where(o => o.FkForumCommentId == id && o.Downvote == 1).Count();
                _context.Forumcomments.Update(comment);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Rating was created and downvoted." });
            }
        }
    }
}