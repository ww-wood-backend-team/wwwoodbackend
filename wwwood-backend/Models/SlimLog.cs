using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace wwwoodbackend.Models
{
    public class SlimLog
    {
        [Key]
        public long LogEntryId { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public LogType LogEntryType{get; set;}


        public class SlimReadLog : SlimLog
        {
            public SlimReadLog(DataRow row)
            {
                LogEntryId = Convert.ToInt32(row["LogEntryId"]);
                Message = row["Message"].ToString();
                Timestamp = row["Timestamp"].ToString();
                LogEntryType = this.LogTypeStringToLogType(row["LogEntryType"].ToString());
            }

            LogType LogTypeStringToLogType(string logType)
            {
                switch (logType)
                {
                    case "Exception":
                        return LogType.Exception;
                    case "Warning":
                        return LogType.Warnning;
                    case "Information":
                        return LogType.Information;
                    default:
                        return LogType.None;
                }
            }

            public long LogEntryId { get; set; }
            public string Message { get; set; }
            public string Timestamp { get; set; }
            public LogType LogEntryType { get; set; }
        }
    }
}
