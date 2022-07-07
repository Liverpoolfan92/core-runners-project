using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Data.Models
{
    public class User : IdentityUser
    {
        public Uri? Image { get; set; } = default!;
        [Required]
        public string Position { get; set; } = default!;

        [Required] 
        public int Age { get; set; }

        public User(string userName) : base(userName)
        {

        }
    }
}
