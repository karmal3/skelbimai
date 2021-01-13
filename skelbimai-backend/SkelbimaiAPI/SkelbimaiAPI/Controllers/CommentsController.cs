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
    public class CommentsController : ControllerBase
    {
        private readonly SkelbimaiDBContext _context;

        public CommentsController(SkelbimaiDBContext context)
        {
            _context = context;
        }
        
                    

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public object GetAdComments([FromRoute] int id)
        {
            try
            {
                var uid = int.Parse(User.Identity.Name);

                return _context.Comments.Where(o => o.FkSkelbimasId == id)
                .Select(o => new
                {
                    o.Id,
                    o.FkUserId,
                    o.LikeCounter,
                    o.Description,
                    o.Date,
                    o.FkSkelbimasId,
                    _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.Username }).FirstOrDefault().Username,
                    o.DislikeCounter,
                    _context.User.Where(x => x.Id == o.FkUserId).Select(c => new { c.ProfilePicture }).FirstOrDefault().ProfilePicture,
                    Disliked = _context.Commentsrating.Where(x => x.FkCommentId == o.Id && x.FkUserId == uid && x.Downvote == 1).Any(),
                    Liked = _context.Commentsrating.Where(x => x.FkCommentId == o.Id && x.FkUserId == uid && x.Upvote == 1).Any()
                })
                .OrderByDescending(o => o.Date)
                .ToList();
            }
            catch
            {
                return _context.Comments.Where(o => o.FkSkelbimasId == id)
                .Select(o => new {
                    o.Id,
                    o.FkUserId,
                    o.LikeCounter,
                    o.Description,
                    o.Date,
                    o.FkSkelbimasId,
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
        public async Task<IActionResult> Add([FromBody] Comments value)
        {
            value.FkUserId = int.Parse(User.Identity.Name);
            value.Date = DateTime.Now;
            _context.Comments.Add(value);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var uid = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == uid).Any();
            var comment = await _context.Comments.FindAsync(id);
            if (uid != comment.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to delete requested comment was not granted" });

            // checks if comment has any ratings
            var count = _context.Commentsrating.Count(o => o.FkCommentId == comment.Id);

            for (int i = 0; i < count; i++)
            {
                var rating = _context.Commentsrating.FirstOrDefault(o => o.FkCommentId == comment.Id);
                _context.Commentsrating.Remove(rating);
                await _context.SaveChangesAsync();
            }

            var comment_delete = _context.Comments.FirstOrDefault(o => o.Id == id);

            _context.Comments.Remove(comment_delete);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Update([FromBody] Comments comment)
        {
            // check if user is the owner of question's quiz
            var uid = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == uid).Any();
            var updatedComment = await _context.Comments.FindAsync(comment.Id);
            if (uid != updatedComment.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to update requested comment was not granted" });

            updatedComment.Description = comment.Description;
            updatedComment.Date = DateTime.Now;

            _context.Update(updatedComment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("up/{id}")]
        public async Task<IActionResult> UpVote([FromRoute] int id)
        {
            var uid = int.Parse(User.Identity.Name);
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return BadRequest(new { message = "Comment was not found" });

            // check if user already upvoted or downvoted the comment
            if (_context.Commentsrating.Where(o => o.FkUserId == uid && o.FkCommentId == comment.Id).Any())
            {
                // user already upvoted or downvoted
                var rating = _context.Commentsrating.Where(o => o.FkUserId == uid && o.FkCommentId == comment.Id).FirstOrDefault();
                if (rating.Upvote == 1)
                {
                    // if comment is already upvoted then delete the rating
                    _context.Commentsrating.Remove(rating);
                    await _context.SaveChangesAsync();
                    var likeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Upvote == 1).Count();
                    comment.DislikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Downvote == 1).Count();
                    comment.LikeCounter = likeCounter;
                    _context.Comments.Update(comment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Rating was removed." });
                }
                else
                {
                    // user hasn't upvoted or has already downvoted the comment

                    rating.Upvote = 1;
                    rating.Downvote = 0;
                    _context.Commentsrating.Update(rating);
                    await _context.SaveChangesAsync();
                    comment.LikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Upvote == 1).Count();
                    comment.DislikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Downvote == 1).Count();
                    _context.Comments.Update(comment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Rating was upvoted." });
                }
            }
            else
            {
                // user hasn't upvoted or downvoted the comment
                Commentsrating rating = new Commentsrating();
                rating.FkUserId = uid;
                rating.FkCommentId = comment.Id;
                rating.Upvote = 1;
                _context.Commentsrating.Add(rating);
                await _context.SaveChangesAsync();
                comment.LikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Upvote == 1).Count();
                comment.DislikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Downvote == 1).Count();
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Rating was created and upvoted." });
            }
        }

        [HttpPost]
        [Route("down/{id}")]
        public async Task<IActionResult> DownVote([FromRoute] int id)
        {
            var uid = int.Parse(User.Identity.Name);
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
                return BadRequest(new { message = "Comment was not found" });

            // check if user already upvoted or downvoted the comment
            if (_context.Commentsrating.Where(o => o.FkUserId == uid && o.FkCommentId == comment.Id).Any())
            {
                // user already upvoted or downvoted
                var rating = _context.Commentsrating.Where(o => o.FkUserId == uid && o.FkCommentId == comment.Id).FirstOrDefault();
                if (rating.Downvote == 1)
                {
                    // if comment is already downvoted then delete the rating
                    _context.Commentsrating.Remove(rating);
                    await _context.SaveChangesAsync();
                    comment.DislikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Downvote == 1).Count();
                    comment.LikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Upvote == 1).Count();
                    _context.Comments.Update(comment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Rating was removed." });
                }
                else
                {
                    // user hasn't downvoted or has already upvoted the comment

                    rating.Upvote = 0;
                    rating.Downvote = 1;
                    _context.Commentsrating.Update(rating);
                    await _context.SaveChangesAsync();
                    comment.DislikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Downvote == 1).Count();
                    comment.LikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Upvote == 1).Count();
                    _context.Comments.Update(comment);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Rating was downvoted." });
                }
            }
            else
            {
                // user hasn't upvoted or downvoted the comment
                Commentsrating rating = new Commentsrating();
                rating.FkUserId = uid;
                rating.FkCommentId = comment.Id;
                rating.Downvote = 1;
                _context.Commentsrating.Add(rating);
                await _context.SaveChangesAsync();
                comment.DislikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Downvote == 1).Count();
                comment.LikeCounter = _context.Commentsrating.Where(o => o.FkCommentId == id && o.Upvote == 1).Count();
                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Rating was created and downvoted." });
            }
        }
    }
}