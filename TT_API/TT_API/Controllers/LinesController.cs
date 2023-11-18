using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTSAPI.Models;

namespace TTSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        public LinesController(ApplicationDbContext _db)
        {
            db = _db;
        }

        [HttpGet("GetLines")]
        public IActionResult GetLines()
        {
            try
            {
                var result = db.Lines.OrderBy(d => d.LineName).ToList();
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetLine/{id}")]
        public IActionResult GetLine(int id)
        {
            try
            {
                var result = db.Lines.SingleOrDefault(d => d.LineID == id);
                if (result != null)
                    return Ok(result);

                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("SaveLine")]
        public IActionResult SaveLine([FromBody] Line Line)
        {
            try
            {
                db.Lines.Add(Line);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("EditLine/{id}")]
        public IActionResult EditLine(int id, [FromBody] Line Line)
        {
            try
            {
                var result = db.Lines.FirstOrDefault(d => d.LineID == id);
                if (result != null)
                {
                    result.LineName = Line.LineName;
                    result.Arr_StationID = Line.Arr_StationID;
                    result.Dep_StationID = Line.Dep_StationID;
                   
                    db.Lines.Update(result);
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
        public IActionResult DeleteLine(int id)
        {
            try
            {
                var result = db.Lines.FirstOrDefault(d => d.LineID == id);
                if (result != null)
                {
                    db.Lines.Remove(result);
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
