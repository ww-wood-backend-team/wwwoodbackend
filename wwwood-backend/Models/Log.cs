using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace wwwoodbackend.Models
{
    public class Log
    {
        [Key]
        public long LogEntryId { get; set; }
        public string Timestamp { get; set; }
        public string LogEntryType { get; set; }
        public string Message { get; set; }
        public string ExceptionJson { get; set; }
        public string FileName { get; set; }
        public string MethodName { get; set; }
        public int LineNumber { get; set; }
        public string SessionId { get; set; }
        public string IpAddress { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }
        public string ZeroClientDescription { get; set; }
        public string ZeroClientName { get; set; }
        public string ZeroClientDivisionId { get; set; }
        public string NotifyEmail { get; set; }
        public string NotifyEmailStatus { get; set; }
        public string NotifyIncident { get; set; }
        public string NotifyIncidentStatus { get; set; }
        public string NotifyTimeout { get; set; }
        public string LogData { get; set; }

    }

    public class ReadLog : Log
    {
        public ReadLog(DataRow row)
        {
            LogEntryId = Convert.ToInt32(row["LogEntryId"]);
            Timestamp = row["Timestamp"].ToString();
            LogEntryType = row["LogEntryType"].ToString();
            Message = row["Message"].ToString();
            ExceptionJson = row["ExceptionJson"].ToString().Replace("\r\n", String.Empty).Replace("\\r\\n", String.Empty);
            FileName = row["FileName"].ToString();
            MethodName = row["MethodName"].ToString();
            LineNumber = Convert.ToInt32(row["LineNumber"]);
            SessionId = row["SessionId"].ToString();
            IpAddress = row["IpAddress"].ToString();
            MachineName = row["MachineName"].ToString();
            UserName = row["UserName"].ToString();
            AssemblyId = row["AssemblyId"].ToString();
            AssemblyName = row["AssemblyName"].ToString();
            AssemblyVersion = row["AssemblyVersion"].ToString();
            ZeroClientDescription = row["ZeroClientDescription"].ToString();
            ZeroClientName = row["ZeroClientName"].ToString();
            ZeroClientDivisionId = row["ZeroClientDivisionId"].ToString();
            NotifyEmail = row["NotifyEmail"].ToString();
            NotifyEmailStatus = row["NotifyEmailStatus"].ToString();
            NotifyIncident = row["NotifyIncident"].ToString();
            NotifyIncidentStatus = row["NotifyIncidentStatus"].ToString();
            NotifyTimeout = row["NotifyTimeout"].ToString();
            LogData = row["LogData"].ToString();

        }



        public long LogEntryId { get; set; }
        public string Timestamp { get; set; }
        public string LogEntryType { get; set; }
        public string Message { get; set; }
        public string ExceptionJson { get; set; }
        public string FileName { get; set; }
        public string MethodName { get; set; }
        public int LineNumber { get; set; }
        public string SessionId { get; set; }
        public string IpAddress { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string AssemblyId { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }
        public string ZeroClientDescription { get; set; }
        public string ZeroClientName { get; set; }
        public string ZeroClientDivisionId { get; set; }
        public string NotifyEmail { get; set; }
        public string NotifyEmailStatus { get; set; }
        public string NotifyIncident { get; set; }
        public string NotifyIncidentStatus { get; set; }
        public string NotifyTimeout { get; set; }
        public string LogData { get; set; }
    }
}


