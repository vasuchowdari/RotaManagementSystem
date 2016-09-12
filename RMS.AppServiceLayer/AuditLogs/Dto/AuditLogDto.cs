using System;

namespace RMS.AppServiceLayer.AuditLogs.Dto
{
    public class AuditLogDto
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string ActionType { get; set; }
        public string TableName { get; set; }
        public long RecordId { get; set; }
    }
}
