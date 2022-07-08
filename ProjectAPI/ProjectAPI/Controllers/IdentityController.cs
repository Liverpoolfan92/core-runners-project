using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers
{
    [Route("controller")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public IdentityController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }
        
        //private readonly AppDbContext _DbContext;

        //  public IdentityController(AppDbContext testDBContext)
        //  {
        //      _DbContext = testDBContext;
        //  }

        //[HttpPost]
        //public async Task<IActionResult> CreateUser(CreateUserInputModel input)
        //{
        //    var newUser = new User(input.Name)
        //    {
        //        UserName = input.Name,
        //        PasswordHash = input.Password,
        //        Email = input.Email

        //    };

        //    //var identityResult = await this._userManager.CreateAsync(user, input.Password);


        //    _DbContext.Users.Add(newUser);
        //    _DbContext.SaveChanges();

        //   return Ok();

        //}

        [HttpPost]
        public async Task<IActionResult> Login (LoginModel input) {
            var user = await this._userManager.FindByNameAsync(input.Email);
            if (user == null)
            {
                return BadRequest(); 
            }

            var passwordValid = await this._userManager.CheckPasswordAsync(user, input.Password);
            if (!passwordValid)
            {
                return BadRequest() ;
            }
            
            return Ok();

        }

    }
}

