using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wwwoodbackend.Models;
using Microsoft.AspNetCore.Cors;

namespace wwwoodbackend.Controllers
{
    [Route("api/logs")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly LogContext _context;

        public LogController(LogContext context)
        {
            _context = context;

            if (_context.Logs.Count() == 0)
            {
                // Create a new LogItem if collection is empty,
                // which means you can't delete all LogItems.
                _context.Logs.Add(new Log { LogEntryId=1, SessionId = "1", AssemblyId="1", NotifyIncident="1"  });
                _context.Logs.Add(new Log { LogEntryId = 2, SessionId = "2", AssemblyId = "2", NotifyIncident = "2" });
                //_context.Logs.Add(new Log { Message = "Item3" });
                _context.SaveChanges();
            }
        }

        //////Refresh every ten seconds
        //public ActionResult Index()
        //{
        //    Response.AddHeader("Refresh", "10");
        //    return View();
        //}

        //filter logs
        //GET: api/logs?from="fdate"&to="tdate"&type="types"&search="search"
        //GET: api/logs?id="id"&name="name"
        /*
        [HttpGet("id={id}&name={name}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<Log>> FilterLogs(long id, string name) {
            //var query = _context.Logs.Where<>
        }*/

        // GET: api/Logs
        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<IEnumerable<Log>>> ListLogs()
        {
            return await _context.Logs.ToListAsync();
        }

        // GET: api/Logs/5
        [HttpGet("{id}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<Log>> SelectLog(long id)
        {
            var log = await _context.Logs.FindAsync();

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }

        // POST: api/Logs
        [HttpPost]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<Log>> PostLog(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(SelectLog), new { id = log.LogEntryId }, log);
        }

        // PUT: api/Logs/5
        [HttpPut("{id}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<IActionResult> PutLogs(long id, Log log)
        {
            if (id != log.LogEntryId)
            {
                return BadRequest();
            }

            _context.Entry(log).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Logs/5
        [HttpDelete("{id}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<IActionResult> DeleteLog(long id)
        {
            var log = await _context.Logs.FindAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}
