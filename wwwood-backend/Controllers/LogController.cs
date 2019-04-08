﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wwwoodbackend.Models;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace wwwoodbackend.Controllers
{
    [Route("api/logs")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    public class LogController : ControllerBase
    {
        //private readonly LogContext _context;
        //private readonly IConfiguration configuration;

        //private SqlConnection _conn;
        private SqlDataAdapter _adapter;
        //public FirstController(IConfiguration config)
        //{
        //    this.configuration = config;
        //}

        //public IEnumerable<Log> Get()
        //{
        //    string connectionString = configuration.GetConnectionString("WWWoodDatabase");
        //    SqlConnection connection = new SqlConnection(connectionString);
        //    connection.Open();
        //    SqlCommand command = new SqlCommand("select (*) from ApplicationLogs");
        //    connection.Close();
        //    return (List < Log >)command.ExecuteScalar();

        //}
        //public IEnumerable<Log> Get()
        //{
        //    _conn = new SqlConnection("Server=13.65.80.168;Database=Auditing;User Id=semocapstone;Password=Capstone2019;Connection Timeout=60");
        //    DataTable _dt = new DataTable();
        //    var query = "select * from ApplicationLogs";
        //    _adapter = new SqlDataAdapter
        //    {
        //        SelectCommand = new SqlCommand(query, _conn)
        //    };

        //    _adapter.Fill(_dt);
        //    List<Log> logs = new List<Models.Log>(_dt.Rows.Count);

        //    if(_dt.Rows.Count>0)
        //    {
        //        foreach(DataRow logrecord in _dt.Rows)
        //        {
        //            logs.Add(new ReadLog(logrecord));
        //        }

        //    }
        //    return logs;

        //}

        //public LogController(LogContext context)
        //{
        //    _context = context;

        //    //if (_context.Logs.Count() == 0)
        //    //{
        //        ////// Create a new LogItem if collection is empty,
        //        //// which means you can't delete all LogItems.
        //        ////_context.Logs.Add(new Log { LogEntryId=1, SessionId = "1", AssemblyId="1", NotifyIncident="1"  });
        //        //_context.Logs.Add(new Log { LogEntryId = 2, SessionId = "2", AssemblyId = "2", NotifyIncident = "2" });
        //        ////_context.Logs.Add(new Log { Message = "Item3" });
        //        //_context.SaveChanges();
        //   // }
        //}

        /////Refresh every ten seconds

        ////Refresh every ten seconds
        //public ActionResult Index()
        //{
        //    Response.AddHeader("Refresh", "10");
        //    return View();
        //}

        ////filter logs
        ///GET: api/logs/?from="fdate"&to="tdate"&type="types"&search="keyword"
        ///Example: https://localhost:44393/api/logs/from=2019-01-01&to=2019-01-20&type=Information&search=dummy
        ///Uses a datetime date (year-mo-da) formatted string to search inbetween two dates ("from" and "to")
        ///searches for a log with LogEntryType that matches the "type"
        ///will search for a log with keyword "keyword" -- not functional yet, I'm not sure this is what it was supposed to be.  I cannot remember.
        ///Not built to withstand incorrect inputs yet, so just make make sure inputs are correct when testing.
        [HttpGet("from={from}&to={to}&type={type}&search={keyword}")]
        [EnableCors("AllowAllHeaders")]
        public IEnumerable<Log> FilterLogs(string from, string to, string type, string keyword){
            using (SqlConnection connection = new SqlConnection("Server=13.65.80.168,1433;Database=Auditing;User Id=semocapstone;Password=Capstone2019"))
            {
                connection.Open();
                DataTable _dt = new DataTable();
                var queryString =
                    //Selects top 10 for now, Simone says there is a timeout issue right now.
                    "select top 10 * from [Auditing].[dbo].[ApplicationLogs]" +
                    //Searches for the log with the matching type
                    "where [LogEntryType] = '" + type + "' and " +
                    //Searches for a log from midnight on the earliest date to ll:59:59PM on the last date
                    "[Timestamp] >= '" + from + "' 00:00:00' and[Timestamp] <= '" + to + "' 23:59:59'; ";
                _adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(queryString, connection) };
                
                //creates the list of matching logs and returns it
                _adapter.Fill(_dt);  /////////////////System.Data.SqlClient.SqlException: 'Incorrect syntax near '00'.'
                List<Log> logs = new List<Models.Log>(_dt.Rows.Count);
                return logs;
                
            }
        }

        //// GET: api/Logs
        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public IEnumerable<SlimLog> ListLogs()
        {
            //string connectionString = configuration.GetConnectionString("WWWoodDatabase");
            //_conn = new SqlConnection("Server=13.65.80.168;Database=Auditing;User Id=semocapstone;Password=Capstone2019;Connection Timeout=60");

            SqlConnection connection = new SqlConnection("Server=13.65.80.168,1433;Database=Auditing;User Id=semocapstone;Password=Capstone2019;Connection Timeout=300");

            connection.Open();

            DataTable _dt = new DataTable();
            //var query = "SELECT * FROM [Auditing].[dbo].[ApplicationLogs]";

            var query = 
            "SELECT TOP (10) " +
            "[LogEntryId]," +
            "[Timestamp]," +
            "[LogEntryType]," +
            "[Message]," +
            "[ExceptionJson]," +
            "[FileName]," +
            "[MethodName]," +
            "[LineNumber]," +
            "[SessionId]," +
            "[IpAddress]," +
            "[MachineName]," +
            "[UserName]," +
            "[AssemblyId]," +
            "[AssemblyName]," +
            "[AssemblyVersion]," +
            "[ZeroClientDescription]," +
            "[ZeroClientName]," +
            "[ZeroClientDivisionId]," +
            "[NotifyEmail]," +
            "[NotifyEmailStatus]," +
            "[NotifyIncident]," +
            "[NotifyIncidentStatus]," +
            "[NotifyTimeout]," +
            "[LogData]" +
            "FROM [Auditing].[dbo].[ApplicationLogs]";
            _adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, connection)
            };

            _adapter.Fill(_dt);
            List<SlimLog> logs = new List<Models.SlimLog>(_dt.Rows.Count);

            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow logrecord in _dt.Rows)
                {
                    logs.Add(new SlimLog.SlimReadLog(logrecord));
                }

            }
            return logs;
        }

        //// GET: api/Logs/5
        [HttpGet("{id}")]
        [EnableCors("AllowAllHeaders")]
        public IEnumerable<Log> SelectLog(long id)
        {
            SqlConnection connection = new SqlConnection("Server=13.65.80.168,1433;Database=Auditing;User Id=semocapstone;Password=Capstone2019;Connection Timeout=60");

            connection.Open();

            DataTable _dt = new DataTable();
            var query = "SELECT * FROM [Auditing].[dbo].[ApplicationLogs] WHERE [LogEntryId] =" +id;
            _adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, connection)
            };

            _adapter.Fill(_dt);

            List<Log> logs = new List<Models.Log>(_dt.Rows.Count);

            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow logrecord in _dt.Rows)
                {
                    logs.Add(new ReadLog(logrecord));
                }
            }

            return logs;
        }

    ////// POST: api/Logs
    //[HttpPost]
    //[EnableCors("AllowAllHeaders")]
    //public async Task<ActionResult<Log>> PostLog(Log log)
    //{
    //    _context.ApplicationLogs.Add(log);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction(nameof(SelectLog), new { id = log.LogEntryId }, log);
    //}

    ////// PUT: api/Logs/5
    //[HttpPut("{id}")]
    //[EnableCors("AllowAllHeaders")]
    //public async Task<IActionResult> PutLogs(long id, Log log)
    //{
    //    if (id != log.LogEntryId)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(log).State = EntityState.Modified;
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

    ////// DELETE: api/Logs/5
    //[HttpDelete("{id}")]
    //[EnableCors("AllowAllHeaders")]
    //public async Task<IActionResult> DeleteLog(long id)
    //{
    //    var log = await _context.ApplicationLogs.FindAsync(id);

    //    if (log == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.ApplicationLogs.Remove(log);
    //    await _context.SaveChangesAsync();

    //    return NoContent();
    //}

}

}
