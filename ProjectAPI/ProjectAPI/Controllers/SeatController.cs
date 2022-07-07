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

        public static bool IsFree(Seat seat, List<Seat> list)
        {
            foreach (Seat s in list)
            {
                if (s.Name == seat.Name)
                {
                    return false;
                }
            }
            return true;

        }

        [HttpPost]
        public IActionResult Create(AddSeatModel_DTO seat)
        {
            var newSeat = new Seat()
            {
                Name = seat.Name,
                Color = seat.Color
            };

            var query = _DbContext.Seats.ToList();

            if (!IsFree(newSeat, query))
            {
                return BadRequest();
            }

            _DbContext.Seats.Add(newSeat);
            _DbContext.SaveChanges();

            return Ok();
        }

        [HttpGet("{Id:int}")]
        public IActionResult Get(int Id)
        {
            var query = _DbContext.Seats
                .Where(seat => seat.Id == Id)
                .ToList();

            if(query.Count <= 0)
            { 
                return BadRequest(); 
            }

            var testData = _DbContext.Seats.Single(x => x.Id == Id);

            return Ok(testData);
        }

        [HttpGet]
        public IActionResult List()
        {
            var query = _DbContext.Seats.ToList();

            if (query.Count <= 0)
            {
                return BadRequest();
            }
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
            var query = _DbContext.Seats
                .Where(seat => seat.Id == Id)
                .ToList();

            if (query.Count == 0)
            {
                return BadRequest();
            }

            var testData = _DbContext.Seats.Single(x => x.Id == Id);

            _DbContext.Seats.Remove(testData);
            _DbContext.SaveChanges();

            return Ok();
        }


    }
}
