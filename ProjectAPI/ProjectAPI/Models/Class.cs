using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Models
{
    public class LoginModel
    {
        
        [EmailAddress]
        [Required]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
