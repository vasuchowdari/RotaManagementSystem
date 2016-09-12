using System;
using System.Linq;

namespace RMS.ServerTests.Moqs
{
    public class Processor
    {
        private readonly DbContextMock _ctx;

        public Processor(DbContextMock ctx)
        {
            _ctx = ctx;
        }

        public AuditLog FindByGuid(Guid id)
        {
            return _ctx.AuditLogs.FirstOrDefault(a => a.Id == id);
        }

        public SystemAccessRole FindSystemAccessRoleById(long id)
        {
            return _ctx.SystemAccessRoles.FirstOrDefault(sar => sar.Id == id);
        }
    }
}
