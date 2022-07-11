using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;

namespace ProjectAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserListController : Controller
    {
        private readonly AppDbContext _DbContext;
        private readonly UserManager<User> _userManager;

        public UserListController(AppDbContext testDBContext, UserManager<User> userManager)
        {
            _DbContext = testDBContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult List()
        {
            var testData = _DbContext.Users.ToList();

            return Ok(testData);
        }
    }
}
