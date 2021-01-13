using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkelbimaiAPI.Entities;
using SkelbimaiAPI.Helpers;

namespace SkelbimaiAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly SkelbimaiDBContext _context;

        public UserController(IUserService userService, IOptions<AppSettings> appSettings, IMapper mapper, SkelbimaiDBContext context)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _context = context;
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> RemoveUser([FromRoute] int id)
        {
            var uid = int.Parse(User.Identity.Name);
            var admin = _context.User.FirstOrDefault(o => o.Id == uid && o.Role == 2);

            if (admin == null)
                return BadRequest(new { message = "Authorization to remove user was not granted." });

            var userToBeRemoved = _context.User.FirstOrDefault(o => o.Id == id);

            if (userToBeRemoved == null)
                return BadRequest(new { message = "User doesn't exist." });

            if (userToBeRemoved.Role == 2)
                return BadRequest(new { message = "Can't remove admins." });

            // check if user have any ads
            var skelbimaiCount = _context.Skelbimas.Count(o => o.FkUserId == userToBeRemoved.Id);
            for (int i = 0; i < skelbimaiCount; i++)
            {
                var skelbimas = _context.Skelbimas.FirstOrDefault(o => o.FkUserId == userToBeRemoved.Id);
                // check if skelbimas has any comments
                var commentsCount = _context.Comments.Count(o => o.FkSkelbimasId == skelbimas.Id);
                for (int j = 0; j < commentsCount; j++)
                {
                    var comment = _context.Comments.FirstOrDefault(o => o.FkSkelbimasId == skelbimas.Id);
                    // check if comment have any ratings
                    var commentratingCount = _context.Commentsrating.Count(o => o.FkCommentId == comment.Id);
                    for (int k = 0; k < commentratingCount; k++)
                    {
                        var commentRating = _context.Commentsrating.FirstOrDefault(o => o.FkCommentId == comment.Id);
                        _context.Commentsrating.Remove(commentRating);
                        await _context.SaveChangesAsync();
                    }
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                }

                // check if skelbimas has any ratings
                var adsRatingCount = _context.Skelbimasrating.Count(o => o.FkSkelbimasId == skelbimas.Id);
                for (int j = 0; j < adsRatingCount; j++)
                {
                    _context.Skelbimasrating.Remove(_context.Skelbimasrating.FirstOrDefault(o => o.FkSkelbimasId == skelbimas.Id));
                    await _context.SaveChangesAsync();
                }

                _context.Skelbimas.Remove(skelbimas);
                await _context.SaveChangesAsync();
            }
            // check if user have any topics
            var topicsCount = _context.Topic.Count(o => o.FkUserId == userToBeRemoved.Id);
            for (int i = 0; i < topicsCount; i++)
            {
                var topic = _context.Topic.FirstOrDefault(o => o.FkUserId == userToBeRemoved.Id);
                // check if topic have any comments
                var commentsCount = _context.Forumcomments.Count(o => o.FkTopicId == topic.Id);
                for (int j = 0; j < commentsCount; j++)
                {
                    var comment = _context.Forumcomments.FirstOrDefault(o => o.FkTopicId == topic.Id);
                    // check if comment have any ratings
                    var commentratingCount = _context.Forumcommentsrating.Count(o => o.FkForumCommentId == comment.Id);
                    for (int k = 0; k < commentratingCount; k++)
                    {
                        var commentRating = _context.Forumcommentsrating.FirstOrDefault(o => o.FkForumCommentId == comment.Id);
                        _context.Forumcommentsrating.Remove(commentRating);
                        await _context.SaveChangesAsync();
                    }
                    _context.Forumcomments.Remove(comment);
                    await _context.SaveChangesAsync();
                }
                _context.Topic.Remove(topic);
                await _context.SaveChangesAsync();
            }

            // check if user is blocked
            if (_context.Blocks.Any(o => o.FkUserId == userToBeRemoved.Id))
            {
                _context.Blocks.Remove(_context.Blocks.FirstOrDefault(o => o.FkUserId == userToBeRemoved.Id));
                await _context.SaveChangesAsync();
            }

            

            _context.User.Remove(userToBeRemoved);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User has been removed" });
        }

        [HttpGet]
        [Route("block/{id}")]
        public async Task<IActionResult> BlockUser([FromRoute] int id, [FromBody] Blocks block)
        {
            var uid = int.Parse(User.Identity.Name);
            var admin = _context.User.FirstOrDefault(o => o.Id == uid && o.Role == 2);

            if (admin == null)
                return BadRequest(new { message = "Authorization to block the user was not granted." });

            var userToBeBlocked = _context.User.FirstOrDefault(o => o.Id == id);

            if (admin.Id == userToBeBlocked.Id)
                return BadRequest(new { message = "You can't block yourself." });

            if (userToBeBlocked == null)
                return BadRequest(new { message = "User doesn't exist." });

            if (_context.Blocks.Any(o => o.FkUserId == userToBeBlocked.Id))
                return BadRequest(new { message = "User is already blocked." });

            if (block.Reason == null)
                return BadRequest(new { message = "Reason can not be empty." });

            block.FkAdminId = admin.Id;
            block.FkUserId = userToBeBlocked.Id;

            _context.Blocks.Add(block);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User was blocked." });
        }

        [HttpGet]
        [Route("userscount")]
        public int UsersCount()
        {
            return _context.User.Count();
        }

        [HttpGet]
        [Route("getuser/{id}")]
        public object GetUser([FromRoute] int id)
        {
            return _context.User.Where(o => o.Id == id).Select(o => new { o.Id, o.Username, o.Role, o.FirstName, o.LastName, o.PhoneNumber, o.Address, o.Email, o.FkCountryId, o.ProfilePicture, o.Date, Blocked = _context.Blocks.Any(z => z.FkUserId == o.Id) }).FirstOrDefault();
            //int uid = int.Parse(User.Identity.Name);
            //if (_context.User.Any(o => o.Id == uid && o.Role == 2))
            //{

            //}
            //return _context.User
            //    .Where(o => o.Id == uid)
            //    .Select(o => new { o.Username, o.FirstName, o.LastName, o.Role, o.PhoneNumber, o.Address, o.Email, o.FkCountryId, o.ProfilePicture, o.Date })
            //    .FirstOrDefault();
        }

        [HttpGet]
        [Route("getusers")]
        public object GetUsers()
        {
            return _context.User.Select(o => new { o.Id, o.Username, o.Role, o.FirstName, o.LastName, o.PhoneNumber, o.Address, o.Email, o.FkCountryId, o.ProfilePicture, o.Date }).ToList();
            //int uid = int.Parse(User.Identity.Name);
            //if (_context.User.Any(o => o.Id == uid && o.Role == 2))
            //{
                
            //}
            //return _context.User
            //    .Where(o => o.Id == uid)
            //    .Select(o => new { o.Username, o.FirstName, o.LastName, o.Role, o.PhoneNumber, o.Address, o.Email, o.FkCountryId, o.ProfilePicture, o.Date })
            //    .FirstOrDefault();
        }

        [HttpGet]
        public object GetUser()
        {
            int uid = int.Parse(User.Identity.Name);
            return _context.User
                .Where(o => o.Id == uid)
                .Select(o => new { o.Username, o.FirstName, o.LastName, o.Role, o.PhoneNumber, o.Address, o.Email, o.FkCountryId, o.ProfilePicture, o.Date })
                .FirstOrDefault();
        }

        [HttpPost]
        [Route("role")]
        public async Task<IActionResult> UpdateUserRole([FromBody] User user)
        {
            var uid = int.Parse(User.Identity.Name);

            var AdminUser = _context.User.FirstOrDefault(o => o.Id == uid && o.Role == 2);

            if (AdminUser == null)
                return BadRequest(new { message = "Access denied to change user's role!" });

            var userToBeUpdated = _context.User.FirstOrDefault(o => o.Id == user.Id);

            if (userToBeUpdated == null)
                return BadRequest(new { message = "User you're trying to change the role doesn't exist!" });

            if (userToBeUpdated.Role == user.Role && user.Role == 2)
                return BadRequest(new { message = "User is already admin!" });
            else if (userToBeUpdated.Role == user.Role && user.Role == 1)
                return BadRequest(new { message = "User is already user!" });

            if (userToBeUpdated.Username == "CheerLeader")
                return BadRequest(new { message = "O gal ne?" });

            userToBeUpdated.Role = user.Role;
            await _context.SaveChangesAsync();
            return Ok(new { message = "User's role was updated." });
        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> UploadPicture([FromBody] User user)
        {
            if (user.Id == 0)
            {
                // user is trying to change profile picture

                var uid = int.Parse(User.Identity.Name);
                var useris = _context.User.FirstOrDefault(o => o.Id == uid);
                useris.ProfilePicture = user.ProfilePicture;

                _context.Update(useris);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                // admin is trying to change user's profile picture

                var uid = int.Parse(User.Identity.Name);
                var AdminUser = _context.User.FirstOrDefault(o => o.Id == uid && o.Role == 2);

                if (AdminUser == null)
                    return BadRequest(new { message = "Access denied to change user's profile picture!" });

                var userToBeUpdated = _context.User.FirstOrDefault(o => o.Id == user.Id && o.Role == 1);

                if (userToBeUpdated == null)
                    return BadRequest(new { message = "User doesn't exist or you're trying to change admin's account information!" });

                userToBeUpdated.ProfilePicture = user.ProfilePicture;

                _context.Update(userToBeUpdated);
                await _context.SaveChangesAsync();
                return Ok();
            }
            
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
        {
            if (user.Id != 0)
            {
                // admin is changing user's information

                int uid = int.Parse(User.Identity.Name);
                var AdminUser = _context.User.FirstOrDefault(o => o.Id == uid && o.Role == 2);

                if (AdminUser == null)
                    return BadRequest(new { message = "Access denied to change user's profile picture!" });

                var userToBeUpdated = _context.User.FirstOrDefault(o => o.Id == user.Id && o.Role == 1);

                if (userToBeUpdated == null)
                    return BadRequest(new { message = "User doesn't exist or you're trying to change admin's account information!" });

                if (user.Username != null)
                {
                    // admin is changing user's username
                    if (_context.User.Any(o => o.Username == user.Username))
                        return BadRequest(new { message = "Username is already taken." });

                    userToBeUpdated.Username = user.Username;

                    _context.Update(userToBeUpdated);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "User's username was updated." });
                }
                else
                {
                    // admin is changing user's First name, Last name, Address, Phone number or Country

                    userToBeUpdated.FirstName = user.FirstName;
                    userToBeUpdated.LastName = user.LastName;
                    userToBeUpdated.Address = user.Address;
                    userToBeUpdated.PhoneNumber = user.PhoneNumber;
                    userToBeUpdated.FkCountryId = user.FkCountryId;

                    _context.User.Update(userToBeUpdated);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "User's info was updated." });
                }
            }
            else
            {
                // user is changing his information

                int uid = int.Parse(User.Identity.Name);
                var useris = _context.User
                    .Where(o => o.Id == uid)
                    .FirstOrDefault();

                if (useris == null)
                    return BadRequest(new { message = "User doesn't exist." });

                if (user.Email != null && user.Password != null)
                {
                    // user is changing password

                    _userService.Update(user.Email, user.Password);

                    return Ok();
                }
                else if (user.Email != null && user.Password == null)
                {
                    // user is changing email

                    if (_context.User.Any(o => o.Email == user.Email))
                        return BadRequest(new { message = "Email is already taken." });

                    useris.Email = user.Email;

                    _context.User.Update(useris);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Email was updated." });
                }
                else if (user.Username != null)
                {
                    // user is changing username

                    if (_context.User.Any(o => o.Username == user.Username))
                        return BadRequest(new { message = "Username is already taken." });

                    useris.Username = user.Username;

                    _context.User.Update(useris);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Username was updated." });
                }
                else
                {
                    // user is changing First name, Last name, Address, Phone number or Country

                    useris.FirstName = user.FirstName;
                    useris.LastName = user.LastName;
                    useris.Address = user.Address;
                    useris.PhoneNumber = user.PhoneNumber;
                    useris.FkCountryId = user.FkCountryId;

                    _context.User.Update(useris);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Profile info was updated." });
                }
            }
            
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("passwordreset")]
        public IActionResult PasswordReset([FromBody] UserDto userDto)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                //ElectronicsNoReply@gmail.com
                //KasKarDal
                Credentials = new NetworkCredential("ElectronicsNoReply@gmail.com", "KasKarDal"),
                EnableSsl = true
            };


            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            // length of the new password
            int size = 8;

            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            string password = result.ToString();

            string body = "Your new password is " + password + "\nYou can change your password on the website.";

            try
            {
                client.Send("ElectronicsNoReply@gmail.com", userDto.Email, "New password", body);
                _userService.Update(userDto.Email, password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                _userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Email, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            var blockedUser = _context.Blocks.FirstOrDefault(o => o.FkUserId == user.Id);
            
            if (blockedUser != null)
            {
                var admin = _context.User.FirstOrDefault(o => _context.Blocks.FirstOrDefault(z => z.FkAdminId == o.Id).FkAdminId == o.Id).Username;
                return BadRequest(new { message = "Your account has been blocked by " + admin + ". Reason = " + blockedUser.Reason });
            } 

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Token = tokenString,
                user.Username,
                user.Role,
                user.Id
            });
        }
    }
}
