using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Models;
using System.Security.Claims;

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

        public static bool IsFree(Booking booking, List<Booking> list)
        {
            foreach (Booking book in list)
            {
                if ((book.Time.Date == booking.Time.Date) && (book.SeatId == booking.SeatId || book.UserId == booking.UserId))
                {
                    return false;
                }
            }
            return true;

        }

        public static bool IsValid(Booking booking,List<User>users, List<Seat> seats)
        {
            int flag = 0;
            foreach(User user in users)
            {
                if(user.Id == booking.UserId) { flag++; }
            }
            
            foreach(Seat seat in seats)
            {
                if(seat.Id == booking.SeatId) { flag++; }   
            }
            if(flag == 2) { return true; }
            return false;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Create(int seat_id,DateTime time)
        {
            var user = User.Identity.IsAuthenticated;

            if (user)
            {
                var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var newBooking = new Booking()
                {
                    UserId = userid,
                    SeatId = seat_id,
                    Time = time.Date

                };

                var usersquery = _DbContext.Users.ToList();
                var seatsquery = _DbContext.Seats.ToList();

                var query = _DbContext.Bookings
                    .Where(book => book.Time.Date == time.Date)
                    .ToList();

                if (!IsValid(newBooking, usersquery, seatsquery))
                {
                    ModelState.AddModelError("Id", "The booking seat_id or user_id is invaild");
                    return BadRequest(ModelState);
                }

                if (!IsFree(newBooking, query))
                {
                    ModelState.AddModelError("SeatId", "The Sead is allready booked for this date");
                    return BadRequest(ModelState);
                }

                _DbContext.Bookings.Add(newBooking);
                _DbContext.SaveChanges();

                return Ok();
            }
            else
            {
                throw new System.UnauthorizedAccessException();
            }
           
        }

        [HttpGet("{dateTime:DateTime}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult List(DateTime dateTime)
        {
            var query = _DbContext.Seats
                .Where(seat => !seat.Bookings.Any(booking => booking.Time.Date == dateTime.Date))
                .ToList();

            return Ok(query);
        }

        [HttpDelete("{Id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Delete(int Id)
        {
            var query = _DbContext.Bookings
                .Where(book => book.Id == Id)
                .ToList();

            if(query.Count == 0) {
                ModelState.AddModelError("Id", "There is no booking with this Id");
                return BadRequest(ModelState);
            }
            var testData = _DbContext.Bookings.Single(x => x.Id == Id);

            _DbContext.Bookings.Remove(testData);
            _DbContext.SaveChanges();

            return Ok();
        }
    }
}

