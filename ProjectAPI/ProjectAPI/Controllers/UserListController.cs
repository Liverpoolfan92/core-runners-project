using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using System.Net;

namespace ProjectAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserListController : Controller
    {
        private readonly AppDbContext _DbContext;

        public UserListController(AppDbContext testDBContext)
        {
            _DbContext = testDBContext;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        public IActionResult Get(string id)
        {
            var testData = _DbContext.Users
                .Where(User => User.Id == id)
                .FirstOrDefault();

            return Ok(testData);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<User>), (int)HttpStatusCode.OK)]
        public IActionResult List()
        {
            var testData = _DbContext.Users.ToList();

            return Ok(testData);
        }

        [HttpDelete]
        public IActionResult Delete(string Id)
        {
            var query = _DbContext.Users
                .Where(user => user.Id == Id)
                .ToList();

            if (query.Count == 0)
            {
                ModelState.AddModelError("Id", "There is no seat with the given Id");
                return BadRequest(ModelState);
            }

            var testData = _DbContext.Users.Single(x => x.Id == Id);

            _DbContext.Users.Remove(testData);
            _DbContext.SaveChanges();

            return Ok();
        }
    }
}
