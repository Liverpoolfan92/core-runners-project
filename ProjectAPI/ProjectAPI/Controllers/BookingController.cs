using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : Controller
    {
        private readonly AppDbContext _DbContext;

        public BookingController(AppDbContext testDBContext)
        {
            _DbContext = testDBContext;
        }

        [HttpPost]
        public IActionResult Create(AddBooking_DTO booking)
        {
            var newBooking = new Booking()
            {
                UserId = booking.UserId,
                SeatId = booking.SeatId,
                Time = booking.Time

            };

            _DbContext.Bookings.Add(newBooking);
            _DbContext.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult List()
        {
            var testData = _DbContext.Bookings.ToList();

            return Ok(testData);
        }
    }



}

