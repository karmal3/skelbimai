using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkelbimaiAPI.Entities;

namespace SkelbimaiAPI
{
    public interface IUserService
    {
        User Authenticate(string email, string password);
        User Create(User user, string password);
        User Update(string email, string password);
        User GetById(int id);
    }
    public class UserService : IUserService
    {
        private SkelbimaiDBContext _context;

        public UserService(SkelbimaiDBContext context)
        {
            _context = context;
        }

        public User Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.User.SingleOrDefault(x => x.Email == email);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.Hash, user.Salt))
                return null;

            // authentication successful
            return user;
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("Username is required");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Email is required");

            if (_context.User.Any(x => x.Username == user.Username))
                throw new Exception("Username " + user.Username + " is already taken");

            if (_context.User.Any(x => x.Email == user.Email))
                throw new Exception("Email " + user.Email + " is already taken");

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Hash = passwordHash;
            user.Salt = passwordSalt;

            _context.User.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User Update(string email, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var useris = _context.User.SingleOrDefault(x => x.Email == email);


            if (useris == null)
                return null;

            useris.Hash = passwordHash;
            useris.Salt = passwordSalt;

            _context.User.Update(useris);
            _context.SaveChanges();

            return useris;
        }

        public User GetById(int id)
        {
            return _context.User.Find(id);
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}