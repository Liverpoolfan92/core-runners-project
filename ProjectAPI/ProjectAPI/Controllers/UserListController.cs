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

        [HttpGet("{Id}")]
        //[Authorize]
        public IActionResult Get(string Id)
        {
            var query = _DbContext.Users
                .Where(user => user.Id == Id)
                .ToList();

            if (query.Count <= 0)
            {
                ModelState.AddModelError("Id", "There is no user with the given Id");
                return BadRequest(ModelState);
            }

            var testData = _DbContext.Users.Single(x => x.Id == Id);

            return Ok(testData);
        }

        [HttpDelete("{Id:int}")]
        //[Authorize]
        public IActionResult Delete(string Id)
        {
            var query = _DbContext.Users
                .Where(user => user.Id == Id)
                .ToList();

            if (query.Count == 0)
            {
                ModelState.AddModelError("Id", "There is no user with this Id");
                return BadRequest(ModelState);
            }
            var testData = _DbContext.Users.Single(x => x.Id == Id);

            _DbContext.Users.Remove(testData);
            _DbContext.SaveChanges();

            return Ok();
        }
    }
}
