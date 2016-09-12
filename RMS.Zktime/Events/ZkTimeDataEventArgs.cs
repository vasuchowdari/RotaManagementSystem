using System;
using System.Collections.Generic;
using RMS.Zktime.Dtos;

namespace RMS.Zktime.Events
{
    public class ZkTimeDataEventArgs : EventArgs
    {
        public ICollection<ZkTimeDataToImportModel> UserClockingData { get; set; }
    }
}
