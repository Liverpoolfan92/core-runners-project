using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Services;
using System.Net;
using System.Security.Claims;

namespace ProjectAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserListController : Controller
    {
        private readonly AppDbContext _DbContext;
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _service;


        public UserListController(AppDbContext testDBContext, UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            _DbContext = testDBContext;
            _userManager = userManager;
            _service = currentUserService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult List()
        {
            var testData = _DbContext.Users.ToList();

            return Ok(testData);
        }

        [HttpGet("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Get()
        {
            var user_id = _service.GetUserId();

            var query = _DbContext.Users
                .Where(user => user.Id == user_id)
                .ToList();

            if (query.Count <= 0)
            {
                ModelState.AddModelError("Id", "There is no user with the given Id");
                return BadRequest(ModelState);
            }

            var testData = _DbContext.Users.Single(x => x.Id == user_id);

            return Ok(testData);
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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
