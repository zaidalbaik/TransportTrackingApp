using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTSAPI.Models;

namespace TTSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        // GET: api/<StationsController>    

        private readonly ApplicationDbContext db;
        public StationsController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet("GetStations")]
        public async Task<IActionResult> GetStations(int pageNumber, int count)
        {
            try
            {
                var stations = await db.Stations.Skip((pageNumber - 1) * count).Take(count).ToListAsync();
                if (stations != null)
                    return Ok(stations);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/<StationController>/5
        [HttpGet("GetStation/{id}")]
        public IActionResult GetStation(int id)
        {
            try
            {
                var result = db.Stations.FirstOrDefault(d => d.StationID == id);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<StationController>
        [HttpPost("SaveStation")]
        public IActionResult SaveStation([FromBody] Station station)
        {
            try
            {
                db.Stations.Add(station);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<StationController>/5
        [HttpPut("EditStation/{id}")]
        public IActionResult EditStation(int id, [FromBody] Station station)
        {
            try
            {
                var result = db.Stations.FirstOrDefault(d => d.StationID == id);
                if (result != null)
                {
                    result.StationName = station.StationName;
                    result.Lat = station.Lat;
                    result.Lon = station.Lon;

                    db.Stations.Update(result);
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

        // DELETE api/<StationController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteStation(int id)
        {
            try
            {
                var result = db.Stations.FirstOrDefault(d => d.StationID == id);
                if (result != null)
                {
                    db.Stations.Remove(result);
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
