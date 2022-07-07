using ProjectAPI.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectAPI.Data.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int SeatId { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("SeatId")]
        public Seat Seat { get; set; }
        public Booking()
        {
        }
    }
}
