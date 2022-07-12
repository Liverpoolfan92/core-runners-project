﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateUser(CreateUserInputModel input)
        {
            var newUser = new User(input.Name);
            newUser.Email = input.Email;

            var tmp1 =  await _userManager.CreateAsync(newUser,input.Password);
            //var tmp2 = await _userManager.AddToRoleAsync(newUser,"User");
            
            return Ok();
        }

        [HttpPut("image")]
        //[Authorize]
        public IActionResult UpdateImage(Uri image,string id)
        {
            try
            {
                var testData = _DbContext.Users.Single(x => x.Id == id);

                testData.Image  = image;

                _DbContext.Users.Update(testData);
                _DbContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("phone")]
        //[Authorize]
        public IActionResult UpdatePhone(string phone, string id)
        {
            try
            {
                var testData = _DbContext.Users.Single(x => x.Id == id);

                testData.PhoneNumber = phone;

                _DbContext.Users.Update(testData);
                _DbContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("age")]
        //[Authorize]
        public IActionResult UpdateAge(int age, string id)
        {
            try
            {
                var testData = _DbContext.Users.Single(x => x.Id == id);

                testData.Age = age;

                _DbContext.Users.Update(testData);
                _DbContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("email")]
        //[Authorize]
        public IActionResult UpdateEmail(string email, string id)
        {
            try
            {
                var testData = _DbContext.Users.Single(x => x.Id == id);

                testData.Email= email;
                testData.NormalizedEmail = email.Normalize().ToUpper();

                _DbContext.Users.Update(testData);
                _DbContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPut("position")]
        //[Authorize]
        public IActionResult UpdatePosition(string position, string id)
        {
            try
            {
                var testData = _DbContext.Users.Single(x => x.Id == id);

                testData.Position = position;

                _DbContext.Users.Update(testData);
                _DbContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no user with the given properties");
                return BadRequest(ModelState);
            }
            return Ok();
        }

    }
}
