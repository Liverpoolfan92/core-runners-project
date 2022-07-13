using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ProjectAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly AppDbContext _DbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterController(AppDbContext testDBContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _DbContext = testDBContext;
            _userManager = userManager;
            _roleManager = roleManager;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
        public async Task<IActionResult> CreateUser(CreateUserInputModel input)
        {
            var newUser = new User(input.Name);
            newUser.Email = input.Email;

            var tmp1 =  await _userManager.CreateAsync(newUser,input.Password);
            //var tmp2 = await _userManager.AddToRoleAsync(newUser,"User");
            
            return Ok();
        }

        [HttpPut("image")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateImage(Uri image)
        {
            try
            {
                var user = User.Identity.IsAuthenticated;

                if (user)
                {
                    var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var testData = _DbContext.Users.Single(x => x.Id == userid);

                    testData.Image = image;

                    _DbContext.Users.Update(testData);
                    _DbContext.SaveChanges();
                }
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("phone")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdatePhone(string phone)
        {
            try
            {
                var user = User.Identity.IsAuthenticated;

                if (user)
                {
                    var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var testData = _DbContext.Users.Single(x => x.Id == userid);

                    testData.PhoneNumber = phone;

                    _DbContext.Users.Update(testData);
                    _DbContext.SaveChanges();
                }
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("age")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateAge(int age)
        {
            try
            {
                var user = User.Identity.IsAuthenticated;
                if (user)
                {
                    var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var testData = _DbContext.Users.Single(x => x.Id == userid);

                    testData.Age = age;

                    _DbContext.Users.Update(testData);
                    _DbContext.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("Id", "Not Authorized");
                    return BadRequest(ModelState);
                }

            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("email")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateEmail(string email)
        {
            try{
                var user = User.Identity.IsAuthenticated;
                if (user)
                {
                    var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var testData = _DbContext.Users.Single(x => x.Id == userid);

                    testData.Email = email;

                    _DbContext.Users.Update(testData);
                    _DbContext.SaveChanges();
                }
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("position")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdatePosition(string position)
        {
            try
            {
                var user = User.Identity.IsAuthenticated;
                if (user)
                {
                    var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var testData = _DbContext.Users.Single(x => x.Id == userid);

                    testData.Position = position;

                    _DbContext.Users.Update(testData);
                    _DbContext.SaveChanges();
                }
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ActivityAsync(DateTime start, DateTime end)
        {
            var user = User.Identity.IsAuthenticated;
            if (user)
            {
                int count = await _DbContext.Bookings.CountAsync<Booking>(book => book.Time.Date > start && book.Time.Date < end && book.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Ok(count);
            }

            throw new UnauthorizedAccessException();
        }


    }
}
