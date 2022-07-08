using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Models;

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

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserInputModel input)
        {
            var newUser = new User(input.Name)
            {
                UserName = input.Name,
                PasswordHash = input.Password,
                Email = input.Email,
                //NormalizedUserName = input.Email.ToUpper()

            };

            var identityResult = await this._userManager.CreateAsync(newUser, input.Password);


            _DbContext.Users.Add(newUser);
            _DbContext.SaveChanges();

           return Ok();

        }
    }
}
