using Microsoft.AspNetCore.Mvc;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Models;

//for admin page

namespace ProjectAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeatController : Controller
    {
        private readonly AppDbContext _DbContext;

        public SeatController(AppDbContext testDBContext)
        {
            _DbContext = testDBContext;
        }

        [HttpPost]
        public IActionResult Create(AddSeatModel_DTO seat)
        {
            var newSeat = new Seat()
            {
                Name = seat.Name,
                Color = seat.Color
            };

            _DbContext.Seats.Add(newSeat);
            _DbContext.SaveChanges();

            /*var test = _DbContext.Bookings
                .Where(x => x.Seat.Name == "sd")
                .Select(x => x.Time)
                .ToList();*/

            return Ok();
        }

        [HttpGet("{Id:int}")]
        //za update i create
        //trqbva samo 1 entity da se vurne
        public IActionResult Get(int Id)
        {
            var testData = _DbContext.Seats.Single(x => x.Id == Id);

            return Ok(testData);
        }

        [HttpGet]
        //all list entities
        //za tablicata s vsichki seatove
        public IActionResult List()
        {
            var testData = _DbContext.Seats.ToList();

            return Ok(testData);
        }


        [HttpPut]
        public IActionResult Update(UpdateSeatModel_DTO seat)
        {
            var testData = _DbContext.Seats.Single(x => x.Id == seat.Id);

            if (testData == null)
            {
                return BadRequest();
            }

            testData.Name = seat.Name;
            testData.Color = seat.Color;

            _DbContext.Seats.Update(testData);
            _DbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete("{Id:int}")]
        public IActionResult Delete(int Id)
        {
            var testData = _DbContext.Seats.Single(x => x.Id == Id);

            _DbContext.Seats.Remove(testData);
            _DbContext.SaveChanges();

            return Ok();
        }


    }
}
