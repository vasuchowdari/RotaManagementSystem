using System;
using System.Data.Entity;

namespace RMS.ServerTests.Moqs
{
    public class DbContextMock : DbContext, IDisposable
    {
        public virtual IDbSet<AuditLog> AuditLogs { get; set; }
        public virtual IDbSet<SystemAccessRole> SystemAccessRoles { get; set; }
    }
}
