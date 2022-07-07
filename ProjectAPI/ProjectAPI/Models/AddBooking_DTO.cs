using ProjectAPI.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPI.Models
{
    public class AddBooking_DTO
    {
        [Required]
        public string UserId { get; set; } = default;
        [Required]
        public int SeatId { get; set; }
        [Required]
        public DateTime Time { get; set; }
    }
}
