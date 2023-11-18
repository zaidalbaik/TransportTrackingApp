using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTSAPI.Models;

namespace TTSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public TripsController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet("GetTrips")]
        public IActionResult GetTrips()
        {
            try
            {
                var result = db.Trips.Include(a => a.Line).OrderBy(d => d.Id).ToList();
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetTrip/{id}")]
        public IActionResult GetTrip(int id)
        {
            try
            {
                var result = db.Trips.Include(b => b.Line).FirstOrDefault(d => d.Id == id);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("SaveTrip")]
        public IActionResult SaveTrip([FromBody] Trip Trip)
        {
            try
            {
                db.Trips.Add(Trip);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("EditTrip/{id}")]
        public IActionResult EditTrip(int id, [FromBody] Trip Trip)
        {
            try
            {
                var result = db.Trips.FirstOrDefault(d => d.Id == id);
                if (result != null)
                {
                    result.Arrival_time = Trip.Arrival_time;
                    result.Departure_time = Trip.Departure_time;
                    result.LineId =Trip.LineId;
                    result.DriverId = Trip.DriverId;
                    db.Trips.Update(result);
                    db.SaveChanges();
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTrip(int id)
        {
            try
            {
                var result = db.Trips.FirstOrDefault(d => d.Id == id);
                if (result != null)
                {
                    db.Trips.Remove(result);
                    db.SaveChanges();
                    return Ok();
                }
                return NotFound();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
