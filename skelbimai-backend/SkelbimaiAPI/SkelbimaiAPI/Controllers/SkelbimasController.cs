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
    public class SkelbimasController : ControllerBase
    {
        private readonly SkelbimaiDBContext _context;

        public SkelbimasController(SkelbimaiDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("rate")]
        public async Task<IActionResult> RateAd ([FromBody] Skelbimasrating rating)
        {
            var uid = int.Parse(User.Identity.Name);

            var ratingExist = _context.Skelbimasrating.FirstOrDefault(o => o.FkSkelbimasId == rating.FkSkelbimasId && o.FkUserId == uid);

            if (ratingExist != null)
                return BadRequest(new { message = "You can only rate ad once." });

            rating.FkUserId = uid;

            _context.Skelbimasrating.Add(rating);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Rating was saved." });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("view/{id}")]
        public async Task<IActionResult> ViewCounter([FromRoute] int id)
        {
            var ad = _context.Skelbimas.FirstOrDefault(o => o.Id == id);

            ad.ViewCounter++;

            _context.Skelbimas.Update(ad);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Route("adcount")]
        public int SkelbimasCount()
        {
            return _context.Skelbimas.Count();
        }

        [HttpGet]
        [Route("graph")]
        public object Graph([FromBody] Graph info)
        {
            var uid = int.Parse(User.Identity.Name);

            switch(info.GraphType)
            {
                // number of ads
                case 1:
                    switch(info.Month)
                    {
                        case 0:
                            return new
                            {
                                Jan = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 1),
                                Feb = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 2),
                                Mar = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 3),
                                Apr = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 4),
                                May = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 5),
                                Jun = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 6),
                                Jul = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 7),
                                Aug = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 8),
                                Sep = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 9),
                                Oct = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 10),
                                Nov = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 11),
                                Dec = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == 12)
                            };
                        default:
                            return new
                            {
                                Week1 = _context.Skelbimas.Count(o => o.Date.Year == info.Year && o.Date.Month == info.Month)
                            };
                    }
                // ratings
                case 2:
                    return new {
                        one = _context.Skelbimasrating.Count(o => o.Rating == 1),
                        two = _context.Skelbimasrating.Count(o => o.Rating == 2),
                        three = _context.Skelbimasrating.Count(o => o.Rating == 3),
                        four = _context.Skelbimasrating.Count(o => o.Rating == 4),
                        five = _context.Skelbimasrating.Count(o => o.Rating == 5)
                    };
                // activity in comments
                case 3:
                    switch (info.Month)
                    {
                        case 0:
                            return new
                            {
                                Jan = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 1) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 1),
                                Feb = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 2) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 2),
                                Mar = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 3) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 3),
                                Apr = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 4) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 4),
                                May = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 5) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 5),
                                Jun = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 6) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 6),
                                Jul = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 7) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 7),
                                Aug = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 8) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 8),
                                Sep = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 9) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 9),
                                Oct = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 10) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 10),
                                Nov = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 11) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 11),
                                Dec = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == 12) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == 12)
                            };
                        default:
                            return new
                            {
                                Week1 = _context.Comments.Count(o => o.Date.Year == info.Year && o.Date.Month == info.Month) + _context.Forumcomments.Count(o => o.Date.Year == info.Year && o.Date.Month == info.Month)
                            };
                    }
                default:
                    return new { message = "default" };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public object Get([FromBody] Filter filter)
        {
            if (filter.Rating == 0)
            {
                return _context.Skelbimas.Select(o => new { o.Id, o.Title, o.Description, o.FkCategoryId, o.FkUserId, o.Price, o.ViewCounter, o.Picture, o.Date, Rating = (decimal?)o.Skelbimasrating.Where(z => o.Id == z.FkSkelbimasId).Sum(z => z.Rating) / (decimal)o.Skelbimasrating.Count(z => o.Id == z.FkSkelbimasId) })
                .OrderByDescending(o => o.Date)
                .ToList();
            }
            else
            {
                var allAds = _context.Skelbimas
                    .Select(o => new { o.Id, o.Title, o.Description, o.FkCategoryId, o.FkUserId, o.Price, o.ViewCounter, o.Picture, o.Date, Rating = (decimal?)o.Skelbimasrating.Where(z => o.Id == z.FkSkelbimasId).Sum(z => z.Rating) / (decimal)o.Skelbimasrating.Count(z => o.Id == z.FkSkelbimasId) })
                    .ToList();

                if (filter.Id != null)
                {
                    allAds = _context.Skelbimas.Where(o => o.FkCategoryId == filter.Id[0])
                        .Select(o => new { o.Id, o.Title, o.Description, o.FkCategoryId, o.FkUserId, o.Price, o.ViewCounter, o.Picture, o.Date, Rating = (decimal?)o.Skelbimasrating.Where(z => o.Id == z.FkSkelbimasId).Sum(z => z.Rating) / (decimal)o.Skelbimasrating.Count(z => o.Id == z.FkSkelbimasId) })
                        .ToList();

                    for (int i = 1; i < filter.Id.Count(); i++)
                    {
                        var skelbimas = _context.Skelbimas.Where(o => o.FkCategoryId == filter.Id[i])
                            .Select(o => new { o.Id, o.Title, o.Description, o.FkCategoryId, o.FkUserId, o.Price, o.ViewCounter, o.Picture, o.Date, Rating = (decimal?)o.Skelbimasrating.Where(z => o.Id == z.FkSkelbimasId).Sum(z => z.Rating) / (decimal)o.Skelbimasrating.Count(z => o.Id == z.FkSkelbimasId) })
                            .ToList();
                        foreach (var skelb in skelbimas)
                        {
                            allAds.Add(skelb);
                        }
                    }
                }

                if ((filter.MinPrice != 0 && filter.MaxPrice == 0) || (filter.MinPrice >= filter.MaxPrice && filter.MinPrice != 0))
                {
                    allAds = allAds.Where(o => o.Price >= filter.MinPrice).ToList();
                }
                else if (filter.MinPrice != 0 && filter.MaxPrice != 0)
                {
                    allAds = allAds.Where(o => o.Price >= filter.MinPrice && o.Price <= filter.MaxPrice).ToList();
                }
                else if (filter.MinPrice == 0 && filter.MaxPrice != 0)
                {
                    allAds = allAds.Where(o => o.Price <= filter.MaxPrice).ToList();
                }


                if (filter.Rating == -1)
                    allAds = allAds.Where(o => o.Rating >= filter.Rating || o.Rating == null).ToList();
                else
                    allAds = allAds.Where(o => o.Rating >= filter.Rating).ToList();

                switch (filter.Date)
                {
                    case 1:
                        allAds = allAds.OrderByDescending(o => o.Date).ToList();
                        break;
                    case 2:
                        allAds = allAds.OrderBy(o => o.Date).ToList();
                        break;
                }

                switch (filter.PriceOrder)
                {
                    case 1:
                        allAds = allAds.OrderBy(o => o.Price).ToList();
                        break;
                    case 2:
                        allAds = allAds.OrderByDescending(o => o.Price).ToList();
                        break;
                }

                return allAds;
            }
        }

        [HttpGet]
        [Route("{id}")]
        public object GetUserAdInfo([FromRoute] int id)
        {
            int uid = int.Parse(User.Identity.Name);
            return _context.Skelbimas.Where(o => o.Id == id && o.FkUserId == uid)
                .Select(o => new { o.Id, o.FkUserId, o.FkCategoryId, o.Description, o.Title, o.Price, o.ViewCounter, o.Picture, o.Date })
                .FirstOrDefault();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("info/{id}")]
        public object GetAdInfo([FromRoute] int id)
        {
            return _context.Skelbimas.Where(o => o.Id == id)
                .Select(o => new { o.Id, o.FkUserId, o.FkCategoryId, o.Description, o.Title, o.Price, o.Picture, o.Date, o.ViewCounter, Category = _context.Category.FirstOrDefault(z => z.Id == o.FkCategoryId).Name, o.FkUser.Username })
                .FirstOrDefault();
        }

        [HttpGet]
        [Route("usersads")]
        public object GetUsersAds()
        {
            int uid = int.Parse(User.Identity.Name);
            return _context.Skelbimas
                .Where(o => o.FkUserId == uid)
                .Select(o => new { o.Title, o.Description, o.Price, o.ViewCounter, o.FkCategoryId, o.Id, o.FkUserId, o.Picture, o.Date })
                .ToList();
        }

        [HttpGet]
        [Route("usersadsadmin/{id}")]
        public object GetUsersAdsAdmin([FromRoute] int id)
        {
            int uid = int.Parse(User.Identity.Name);
            var admin = _context.User.FirstOrDefault(o => o.Id == uid);

            if (admin == null)
                return null;

            return _context.Skelbimas
                .Where(o => o.FkUserId == id)
                .Select(o => new { o.Title, o.Description, o.Price, o.ViewCounter, o.FkCategoryId, o.Id, o.FkUserId, o.Picture, o.Date })
                .ToList();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] Skelbimas value)
        {
            value.FkUserId = int.Parse(User.Identity.Name);
            _context.Skelbimas.Add(value);
            await _context.SaveChangesAsync();
            return Ok(new { id = value.Id, message = "Ad created successfully." });
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] Skelbimas value)
        {
            // check if user is the owner of the skelbimas
            var id = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == id).Any();
            var skelbimas = await _context.Skelbimas.FindAsync(value.Id);
            if (id != skelbimas.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to delete requested skelbimas was not granted" });

            // checks if skelbimas has any comments
            var count = _context.Comments.Count(o => o.FkSkelbimasId == value.Id);

            for (int i = 0; i < count; i++)
            {
                var comments = _context.Comments.FirstOrDefault(o => o.FkSkelbimasId == value.Id);
                var COUNT = _context.Commentsrating.Count(o => o.FkCommentId == comments.Id);
                for (int j = 0; j < COUNT; j++)
                {
                    var commentRating = _context.Commentsrating.FirstOrDefault(o => o.FkCommentId == comments.Id);
                    _context.Commentsrating.Remove(commentRating);
                    await _context.SaveChangesAsync();
                }
                _context.Comments.Remove(comments);
                await _context.SaveChangesAsync();
            }

            // checks if skelbimas has any ratings
            var adsRatingCount = _context.Skelbimasrating.Count(o => o.FkSkelbimasId == value.Id);
            for (int i = 0; i < adsRatingCount; i++)
            {
                _context.Skelbimasrating.Remove(_context.Skelbimasrating.FirstOrDefault(o => o.FkSkelbimasId == value.Id));
                await _context.SaveChangesAsync();
            }

            var skelbimas_delete = _context.Skelbimas.FirstOrDefault(o => o.Id == value.Id);

            _context.Skelbimas.Remove(skelbimas_delete);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Update([FromBody] Skelbimas skelbimas)
        {
            var id = int.Parse(User.Identity.Name);
            // role = true if user is admin, false if not
            var role = _context.User.Where(o => o.Role == 2 && o.Id == id).Any();
            var updatedSkelbimas = await _context.Skelbimas.FindAsync(skelbimas.Id);

            // check if user is the owner of the skelbimas
            if (id != updatedSkelbimas.FkUserId)
                if (role == false)
                    return BadRequest(new { message = "Authorization to edit requested skelbimas was not granted" });

            if (skelbimas.Picture == null)
            {
                updatedSkelbimas.Title = skelbimas.Title;
                updatedSkelbimas.Description = skelbimas.Description;
                updatedSkelbimas.Price = skelbimas.Price;
                updatedSkelbimas.FkCategoryId = skelbimas.FkCategoryId;
                updatedSkelbimas.Date = DateTime.Now;
            }
            else
            {
                updatedSkelbimas.Title = skelbimas.Title;
                updatedSkelbimas.Description = skelbimas.Description;
                updatedSkelbimas.Price = skelbimas.Price;
                updatedSkelbimas.FkCategoryId = skelbimas.FkCategoryId;
                updatedSkelbimas.Date = DateTime.Now;
                updatedSkelbimas.Picture = skelbimas.Picture;
            }

            _context.Skelbimas.Update(updatedSkelbimas);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}