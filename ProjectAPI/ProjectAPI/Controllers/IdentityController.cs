using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProjectAPI.Controllers
{
    [Route("controller")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;

        }


        private async Task<string> CreateToken(User user)
        {

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id)
            };
            if (roles.Any(r => r == "Admin"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                claims: claims, 
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                
            return jwt;
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login (LoginModel input) {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Error in email check");
                return BadRequest(ModelState);
            }

            var passwordValid = await this._userManager.CheckPasswordAsync(user, input.Password);
            if (!passwordValid)
            {
                ModelState.AddModelError("", "Error in password");
                return BadRequest(ModelState);
            }
            var token = await CreateToken(user);
            return Ok(token);

        }
    }
}

