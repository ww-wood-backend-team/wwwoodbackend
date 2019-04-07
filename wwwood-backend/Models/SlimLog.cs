using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace wwwoodbackend.Models
{
    public class SlimLog
    {
        [Key]
        public long LogEntryId { get; set; }
        public string Timestamp { get; set; }
        public string LogEntryType
        {
            get; set;

        }


        public class SlimReadLog : SlimLog
        {
            public SlimReadLog(DataRow row)
            {
                LogEntryId = Convert.ToInt32(row["LogEntryId"]);
                Timestamp = row["Timestamp"].ToString();
                LogEntryType = row["LogEntryType"].ToString();
            }

            public long LogEntryId { get; set; }
            public string Timestamp { get; set; }
            public string LogEntryType { get; set; }
        }
    }
}
