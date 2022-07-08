using System.ComponentModel.DataAnnotations;

namespace ProjectAPI.Models
{
    public class CreateUserInputModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        [Required]
        public string Name { get; set; }

    }
}
