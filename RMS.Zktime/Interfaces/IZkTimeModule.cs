using System;
using RMS.Zktime.Events;

namespace RMS.Zktime.Interfaces
{
    public interface IZkTimeModule
    {
        void Init(bool firstRun, DateTime mostRecentClockingTime);
        event EventHandler<ZkTimeDataEventArgs> ZkTimeDataReadyEvent;
        event EventHandler NoNewZkTimeDataEvent;
    }
}
