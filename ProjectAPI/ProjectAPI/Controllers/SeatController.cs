﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;
using ProjectAPI.Hubs;
using ProjectAPI.Models;

//for admin page

namespace ProjectAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SeatController : Controller
    {
        private readonly AppDbContext _DbContext;
        private readonly IHubContext<SignalHub> _hubContext; 

        public SeatController(AppDbContext testDBContext, IHubContext<SignalHub> hubContext)
        {
            _DbContext = testDBContext;
            _hubContext = hubContext;
        }

        //part of error handling
        public static bool NameIsFree(Seat seat, List<Seat> list)
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

        //signaR


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult Create(AddSeatModel_DTO seat)
        {
            var newSeat = new Seat()
            {
                Name = seat.Name,
                Color = seat.Color
            };

            var query = _DbContext.Seats.ToList();

            if (!NameIsFree(newSeat, query))
            {
                ModelState.AddModelError("Name", "There is a seat with the same name, so you cant add this one");
                return BadRequest(ModelState);
            }
             
            _DbContext.Seats.Add(newSeat);
            _DbContext.SaveChanges();

            _hubContext.Clients.All.SendAsync("ReceiveSeat", newSeat);

            return Ok();
        }

        [HttpGet("{Id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult Get(int Id)
        {
            var query = _DbContext.Seats
                .Where(seat => seat.Id == Id)
                .ToList();

            if(query.Count <= 0)
            {
                ModelState.AddModelError("Id", "There is no seat with the given Id");
                return BadRequest(ModelState); 
            }

            var testData = _DbContext.Seats.Single(x => x.Id == Id);

            return Ok(testData);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult List()
        {
            //var query = _DbContext.Seats.ToList();

            //if (query.Count <= 0)
            //{
            //    ModelState.AddModelError("", "There are no seats added");
            //    return BadRequest(ModelState);
            //}
            var testData = _DbContext.Seats.ToList();

            return Ok(testData);
        }


        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult Update(UpdateSeatModel_DTO seat)
        {
            try
            {
                var testData = _DbContext.Seats.Single(x => x.Id == seat.Id);

                testData.Name = seat.Name;
                testData.Color = seat.Color;

                _DbContext.Seats.Update(testData);
                _DbContext.SaveChanges();
            }
            catch (InvalidOperationException)
            {
                ModelState.AddModelError("Id", "There is no seat with the given properties");
                return BadRequest(ModelState);
            }


            return Ok();
        }

        [HttpDelete("{Id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public IActionResult Delete(int Id)
        {
            var query = _DbContext.Seats
                .Where(seat => seat.Id == Id)
                .ToList();

            if (query.Count == 0)
            {
                ModelState.AddModelError("Id", "There is no seat with the given Id");
                return BadRequest(ModelState);
            }

            var testData = _DbContext.Seats.Single(x => x.Id == Id);

            _DbContext.Seats.Remove(testData);
            _DbContext.SaveChanges();

            return Ok();
        }


    }
}
