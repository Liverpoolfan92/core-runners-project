using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Data.Models
{
    public class User : IdentityUser
    {
        public Uri? Image { get; set; } = default!;


        public string? Position { get; set; } = default!;


        public int Age { get; set; }

        public User(string userName) : base(userName)
        {

        }
    }
}