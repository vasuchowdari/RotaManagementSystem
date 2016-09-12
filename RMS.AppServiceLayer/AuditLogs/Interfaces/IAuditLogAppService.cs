using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.AuditLogs.Dto;

namespace RMS.AppServiceLayer.AuditLogs.Interfaces
{
    public interface IAuditLogAppService : IDisposable
    {
        // Service Methods
        void Audit(string actionType, string tableName, long userId, long recordId);

        // Repo Methods
        ICollection<AuditLogDto> GetAllAuditLogs();

        // CRUD
        void Create(AuditLogDto auditLogDto);
    }
}
