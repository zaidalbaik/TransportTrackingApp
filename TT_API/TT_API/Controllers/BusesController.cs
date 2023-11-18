using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TTSAPI.Models;

namespace TTSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class BusesController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public BusesController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet("GetBuses")]
        public IActionResult GetBuses()
        {
            try
            {
                var result = db.Buses.ToList();
                if (result != null)
                    return Ok(result);
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("GetBus/{id}")]
        public IActionResult GetBus(int id)
        {
            try
            {
                var result = db.Buses.SingleOrDefault(b => b.BusID == id);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetSetOfActiveBuses/{id}")]
        public IActionResult GetSetOfActiveBuses(int id)
        {
            try
            {
                var result = db.Buses.Where(d => d.LineId == id && d.IsActive == true).ToList();
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("GetSetOfBuses")]
        public IActionResult GetSetOfBuses([FromBody] List<int> listOfIds)
        {
            try
            {
                List<Bus> SetOfBuses = new List<Bus>();
                foreach (var id in listOfIds)
                {
                    var result = db.Buses.SingleOrDefault(b => b.BusID == id);
                    if (result != null)
                        SetOfBuses.Add(result);

                }
                if (SetOfBuses != null)
                    return Ok(SetOfBuses);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("SaveBus")]
        public IActionResult SaveBus([FromBody] Bus bus)
        {

            try
            {
                db.Buses.Add(bus);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [HttpPut("EditBus/{id}")]
        public IActionResult EditBus(int id, [FromBody] Bus bus)
        {
            try
            {
                var result = db.Buses.FirstOrDefault(b => b.BusID == id);

                if (result != null)
                {
                    result.Lat = bus.Lat;
                    result.Lon = bus.Lon;
                    result.LineId = bus.LineId;
                    result.IsActive = bus.IsActive;

                    db.Buses.Update(result);
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

        [HttpPut("EditLatLonForBus/{id}")]
        public IActionResult EditLatLonForBus(int id, [FromBody] Bus bus)
        {
            try
            {
                var result = db.Buses.FirstOrDefault(b => b.BusID == id);

                if (result != null)
                {
                    result.Lat = bus.Lat;
                    result.Lon = bus.Lon;

                    db.Buses.Update(result);
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

        [HttpPut("EditActivityForBus/{id}")]
        public IActionResult EditActivityForBus(int id, [FromBody] Bus bus)
        {
            try
            {
                var result = db.Buses.FirstOrDefault(b => b.BusID == id);

                if (result != null)
                {
                    result.IsActive = bus.IsActive;
                    db.Buses.Update(result);
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
        public IActionResult DeleteBus(int id)
        {
            try
            {
                var result = db.Buses.FirstOrDefault(b => b.BusID == id);
                if (result != null)
                {
                    db.Buses.Remove(result);
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
