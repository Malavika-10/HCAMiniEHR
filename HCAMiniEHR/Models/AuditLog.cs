using System;

namespace HCAMiniEHR.Models
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }

        public string TableName { get; set; }

        public string Operation { get; set; }   // INSERT, UPDATE, DELETE

        public string RecordId { get; set; }

        public DateTime ChangedOn { get; set; }

        public string ChangedBy { get; set; }
    }
}
