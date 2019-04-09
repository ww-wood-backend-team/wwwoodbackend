using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using wwwoodbackend.Models;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
using System.Data;
using System;

namespace wwwoodbackend.Controllers
{
    [Route("api/logs")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    public class LogController : ControllerBase
    {

        private SqlDataAdapter _adapter;

        /////Refresh every ten seconds

        ////Refresh every ten seconds
        //public ActionResult Index()
        //{
        //    Response.AddHeader("Refresh", "10");
        //    return View();
        //}


        ////filter logs
        //GET: api/logs/?from="fdate"&to="tdate"&type="types"&search="keyword"
        //Example: https://localhost:44393/api/logs/from=2019-01-01&to=2019-01-20&type=Information&search=dummy
        ///Uses a datetime date (year-mo-da) formatted string to search inbetween two dates ("from" and "to")
        ///searches for a log with LogEntryType that matches the "type"
        ///will search for a log with keyword "keyword" -- not functional yet, I'm not sure this is what it was supposed to be.  I cannot remember.
        ///Not built to withstand incorrect inputs yet, so just make make sure inputs are correct when testing.
        [HttpGet("from={from}&to={to}&type={type}&search={keyword}")]

        [EnableCors("AllowAllHeaders")]
        public IEnumerable<SlimLog> FilterLogs(string from,string to,string type,string keyword) 
        {

            SqlConnection connection = new SqlConnection("Server=13.65.80.168,1433;Database=Auditing;User Id=semocapstone;Password=Capstone2019;Connection Timeout=300");
            connection.Open(); 
            DataTable _dt = new DataTable();

            //Selects top 10 for now, Simone says there is a timeout issue right now.


            //Example: /api/logs/from=2019-01-01&to=2019-01-20&type=Information&search=false
            var queryStringFilterall =
                "SELECT TOP (10) * FROM [Auditing].[dbo].[ApplicationLogs] WHERE [LogEntryType] = '" + type + "' AND [Timestamp] >= '" + from + " 00:00:00 ' AND [Timestamp] <= '" + to + " 23:59:59 ' AND [Message] LIKE '%" + keyword+"%'";

            //Example: /api/logs/from=2019-01-01&to=2019-01-20&type=null&search=null
            var queryStringFilterDate =
                "SELECT TOP (10) * FROM [Auditing].[dbo].[ApplicationLogs] WHERE [Timestamp] >= '" + from + " 00:00:00 ' AND [Timestamp] <= '" + to + " 23:59:59 '";

            //Example: /api/logs/from=null&to=null&type=null&search=false
            var queryStringFilterSearch =
                "SELECT TOP (10) * FROM [Auditing].[dbo].[ApplicationLogs] WHERE " +
                "[Message]" +
                "LIKE '%" + keyword + "%'";

            //Example: /api/logs/from=null&to=null&type=information&search=null
            var queryStringFilterType =
                "SELECT TOP (10) * FROM [Auditing].[dbo].[ApplicationLogs] WHERE [LogEntryType] = '" + type + "'";

            //Example: /api/logs/from=2019-01-01&to=2019-01-20&type=information&search=null
            var queryStringFilterNoSearch =
                "SELECT TOP (10) * FROM [Auditing].[dbo].[ApplicationLogs] WHERE [LogEntryType] = '" + type + "' AND [Timestamp] >= '" + from + " 00:00:00 ' AND [Timestamp] <= '" + to + " 23:59:59 '";

            //Example: /api/logs/from=2019-01-01&to=2019-01-20&type=null&search=false
            var queryStringFilterNoType =
                "SELECT TOP (10) * FROM [Auditing].[dbo].[ApplicationLogs] WHERE [Timestamp] >= '" + from + " 00:00:00 ' AND [Timestamp] <= '" + to + " 23:59:59 ' AND [Message] LIKE '%" + keyword + "%'";

            //Example: /api/logs/from=null&to=null&type=Information&search=false
            var queryStringFilterNoDate =
                "SELECT TOP (10) * FROM [Auditing].[dbo].[ApplicationLogs] WHERE [LogEntryType] = '" + type + "' AND [Message] LIKE '%" + keyword + "%'";



            if (type.Equals("null", StringComparison.InvariantCultureIgnoreCase) && keyword.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                _adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryStringFilterDate, connection)
                };
            }

            else if(type.Equals("null", StringComparison.InvariantCultureIgnoreCase) && to.Equals("null", StringComparison.InvariantCultureIgnoreCase) && from.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                _adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryStringFilterSearch, connection)
                };
            }
            else if(to.Equals("null", StringComparison.InvariantCultureIgnoreCase) && from.Equals("null", StringComparison.InvariantCultureIgnoreCase) && keyword.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                _adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryStringFilterType, connection)
                };
            }
            else if (!type.Equals("null", StringComparison.InvariantCultureIgnoreCase) && !to.Equals("null", StringComparison.InvariantCultureIgnoreCase) && !from.Equals("null", StringComparison.InvariantCultureIgnoreCase) && keyword.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                _adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryStringFilterNoSearch, connection)
                };
            }
            else if (!keyword.Equals("null", StringComparison.InvariantCultureIgnoreCase) && !to.Equals("null", StringComparison.InvariantCultureIgnoreCase) && !from.Equals("null", StringComparison.InvariantCultureIgnoreCase) && type.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                _adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryStringFilterNoType, connection)
                };
            }
            else if (!keyword.Equals("null", StringComparison.InvariantCultureIgnoreCase) && !type.Equals("null", StringComparison.InvariantCultureIgnoreCase) && to.Equals("null", StringComparison.InvariantCultureIgnoreCase) && from.Equals("null", StringComparison.InvariantCultureIgnoreCase))
            {
                _adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryStringFilterNoDate, connection)
                };
            }
            else
            {
                _adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(queryStringFilterall, connection)
                };
            }

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

        //// GET: api/Logs
        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public IEnumerable<SlimLog> ListLogs()
        {


            SqlConnection connection = new SqlConnection("Server=13.65.80.168,1433;Database=Auditing;User Id=semocapstone;Password=Capstone2019;Connection Timeout=300");

            connection.Open();

            DataTable _dt = new DataTable();
            //var query = "SELECT * FROM [Auditing].[dbo].[ApplicationLogs]";

            var query1 = 
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
                SelectCommand = new SqlCommand(query1, connection)
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


    }

}
