using System;

namespace RMS.ServerTests.Moqs
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string ActionType { get; set; }
        public string TableName { get; set; }
        public long RecordId { get; set; }
    }
}
