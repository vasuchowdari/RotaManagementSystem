using System;
using System.Collections.Generic;

namespace RMS.AppServiceLayer.Zktime.Interfaces
{
    public interface IZktRecordAppService : IDisposable
    {
        ICollection<string> GetUnmatchedByEmployeeName(string employeeName, DateTime startDateTime, DateTime endDateTime);
        void SetIsMatched(DateTime clockInDateTime, DateTime clockOutDateTime);
    }
}
