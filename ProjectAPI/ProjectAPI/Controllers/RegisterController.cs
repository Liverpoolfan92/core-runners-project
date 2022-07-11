using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Models;
using System.Security.Cryptography;

namespace ProjectAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly AppDbContext _DbContext;
        private readonly UserManager<User> _userManager;

        public RegisterController(AppDbContext testDBContext, UserManager<User> userManager)
        {
            _DbContext = testDBContext;
            _userManager = userManager;
        }


        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        [HttpPost]
        public IActionResult CreateUser(CreateUserInputModel input)
        {
            var newUser = new User(input.Email);
            newUser.NormalizedUserName = newUser.UserName.Normalize().ToUpper();
            newUser.PasswordHash = HashPassword(input.Password);

            _DbContext.Users.Add(newUser);
            _DbContext.SaveChanges();
            //_DbContext.Users.Add(newUser);

           return Ok();
        }

    }
}
