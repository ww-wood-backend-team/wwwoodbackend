using System;
namespace wwwoodbackend.Models
{
    public class Log
    {
        [System.ComponentModel.DataAnnotations.Key]
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
