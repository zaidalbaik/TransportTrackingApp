using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Primitives;
using TTSAPI.Models;

namespace TTSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public UsersController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var result = db.Users.OrderBy(u => u.UserID).ToList();
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("GetUser/{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var result = db.Users.SingleOrDefault(u => u.UserID == id);
                if (result != null)
                    return Ok(result);

                return NotFound();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("IsStoredAccount")]
        public async Task<IActionResult> IsStoredAccount(string email, string password)
        {
            try
            {
                var result = await db.Users.SingleOrDefaultAsync(u => u.Email == email);

                if (result != null)
                {
                    if (result.Password == password.Trim())
                    {
                        return Ok(result);
                    }
                    return NotFound();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("IsStoredEmail")]
        public IActionResult IsStoredEmail(string email)
        {
            try
            {
                var result = db.Users.SingleOrDefault(u => u.Email == email.Trim());
                if (result != null)
                    return Ok();

                return NotFound();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("SaveUser")]
        public IActionResult SaveUser([FromBody] User user)
        {
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                return Ok();

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut("EditUser/{id}")]
        public IActionResult EditUser(int id, [FromBody] User user)
        {
            try
            {
                var result = db.Users.SingleOrDefault(u => u.UserID == id);

                if (result != null)
                {
                    result.FirstName = user.FirstName;
                    result.LastName = user.LastName;
                    db.Users.Update(result);
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

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var Result = db.Users.SingleOrDefault(u => u.UserID == id);
                if (Result != null)
                {
                    db.Users.Remove(Result);
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
