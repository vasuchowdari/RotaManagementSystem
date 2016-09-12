using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using RMS.Zktime.Classes;
using RMS.Zktime.Dtos;
using RMS.Zktime.Events;
using RMS.Zktime.Interfaces;

namespace RMS.Zktime.Services
{
    public class ZkTimeModule : IZkTimeModule
    {
        private DateTime _lastClockingTimeValue;
        private readonly DataContext _zktctx;
        //private readonly DataContext _rmsctx;
        private bool _firstRun;
        private const int _minusNMonths = -3;

        public delegate void EventHandler(object sender, ZkTimeDataEventArgs args);
        public event EventHandler<ZkTimeDataEventArgs> ZkTimeDataReadyEvent = delegate { };
        public event System.EventHandler NoNewZkTimeDataEvent = delegate { };

        public ZkTimeModule()
        {
            _zktctx = new DataContext(ConfigurationManager.ConnectionStrings["zktimedb"].ConnectionString);
            //_rmsctx = new DataContext(ConfigurationManager.ConnectionStrings["RmsContext"].ConnectionString);
            _firstRun = true;
        }

        public void Init(bool firstRun, DateTime mostRecentClockingTime)
        {
            if (!firstRun)
            {
                _firstRun = false;
            }

            var dataToImport = new List<ZkTimeDataToImportModel>();
            var maxDate = DateTime.Now;

            if (!_firstRun)
            {
                if (DateTime.Equals(_lastClockingTimeValue, DateTime.MinValue))
                {
                    if (!DateTime.Equals(mostRecentClockingTime, DateTime.MinValue))
                    {
                        maxDate = mostRecentClockingTime;
                    }
                }
                else
                {
                    maxDate = _lastClockingTimeValue;
                }
            }
            else
            {
                maxDate = maxDate.AddMonths(_minusNMonths);
            }

            _firstRun = false;

            // clocking in/out terminal IDs as set in the ZKTimeDB
            //var machineNumbers = new List<int> { 12, 22, 32, 42, 62, 72, 73, 82 };
            var machineNumbers = new List<int> { 72, 73, 82 }; // Baldock only

            var zkUserRecords = _zktctx.GetTable<ZkUser>();
            var machineRecords = _zktctx.GetTable<Machine>();
            var clockingRecords = _zktctx.GetTable<Clocking>();

            // leave to last possible line to get closest to now
            var minDate = DateTime.Now;

            try
            {
                var q =
                    from c in clockingRecords
                    join u in zkUserRecords on c.UserId equals u.UserId
                    join m in machineRecords on c.MachineNumber equals m.MachineNumber
                    where machineNumbers.Contains(m.MachineNumber)
                          && c.ClockingTime > maxDate
                          && c.ClockingTime <= minDate
                    select new ZkTimeDataToImportModel
                    {
                        SiteId = m.MachineNumber,
                        SiteName = m.MachineAlias,
                        StaffId = int.Parse(u.BadgeNumber),
                        StaffName = u.Name,
                        ClockingTime = c.ClockingTime
                    };

                dataToImport.AddRange(q);

                dataToImport = dataToImport.OrderBy(zkt => zkt.ClockingTime)
                                           .ToList();

                _lastClockingTimeValue = dataToImport.Select(zkt => zkt.ClockingTime)
                                                     .LastOrDefault();

                if (dataToImport.Count > 0)
                {
                    var dataToReturnForImport = new ZkTimeDataEventArgs
                    {
                        UserClockingData = dataToImport
                    };

                    ZkTimeDataReadyEvent(null, dataToReturnForImport);
                }
                else
                {
                    NoNewZkTimeDataEvent(null, null);
                }
            }
            catch (SqlException ex)
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}