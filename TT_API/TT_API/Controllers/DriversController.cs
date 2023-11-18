using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTSAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class DriversController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public DriversController(ApplicationDbContext _db)
        {
            db = _db;
        }

        // GET: api/<DriverController>
        [HttpGet("GetDrivers")]
        public IActionResult GetDrivers()
        {
            try
            {
                var result = db.Drivers.Include(d => d.Bus).OrderBy(d => d.Id).ToList();
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET api/<DriverController>/5
        [HttpGet("GetDriver/{id}")]
        public IActionResult GetDriver(int id)
        {
            try
            {
                var result = db.Drivers.Include(b => b.Bus).SingleOrDefault(d => d.Id == id);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }       

        // GET api/<DriverController>/{email},{password}
        [HttpGet("GetDriver")]
        public IActionResult GetDriver(string email, string password)
        {
            try
            {
                var result = db.Drivers.Include(b => b.Bus).SingleOrDefault(b => b.Email == email && b.Password == password);

                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/<DriverController>
        [HttpPost("SaveDriver")]
        public IActionResult SaveDriver([FromBody] Driver driver)
        {
            try
            {
                db.Drivers.Add(driver);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT api/<DriverController>/5
        [HttpPut("EditDriver/{id}")]
        public IActionResult EditDriver(int id, [FromBody] Driver driver)
        {
            try
            {
                var result = db.Drivers.SingleOrDefault(d => d.Id == id);
                if (result != null)
                {
                    result.DriverName = driver.DriverName;
                    db.Drivers.Update(result);
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

        // DELETE api/<DriverController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteDriver(int id)
        {
            try
            {
                var result = db.Drivers.SingleOrDefault(d => d.Id == id);
                if (result != null)
                {
                    db.Drivers.Remove(result);
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
