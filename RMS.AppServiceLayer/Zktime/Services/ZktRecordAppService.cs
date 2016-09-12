using System;
using System.Collections.Generic;
using System.Linq;
using RMS.AppServiceLayer.Zktime.Interfaces;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Zktime.Services
{
    public class ZktRecordAppService : IZktRecordAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();


        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public ICollection<string> GetUnmatchedByEmployeeName(string employeeName, DateTime startDateTime, DateTime endDateTime)
        {
            // These ranges can be pulled from the SysConfig thresholds also
            var startRange = startDateTime.AddHours(-2);
            var endRange = endDateTime.AddHours(5);

            var unmatchedRecords = _unitOfWork.ZkTimeClockingRecordRepository
                .Get(z => z.ZkTimeUserName == employeeName &&
                          z.ClockingTime >= startRange &&
                          z.ClockingTime <= endRange &&
                          !z.IsMatched)
                .Select(z => z.ClockingTime.ToString())
                .ToList();

            return unmatchedRecords;
        }

        public void SetIsMatched(DateTime clockInDateTime, DateTime clockOutDateTime)
        {
            var zktClockingInRecord = _unitOfWork.ZkTimeClockingRecordRepository.Get(z => z.ClockingTime == clockInDateTime)
                                                                                .FirstOrDefault();

            var zktClockingOutRecord = _unitOfWork.ZkTimeClockingRecordRepository.Get(z => z.ClockingTime == clockOutDateTime)
                                                                                 .FirstOrDefault();

            if (zktClockingInRecord != null)
            {
                zktClockingInRecord.IsMatched = true;
                _unitOfWork.ZkTimeClockingRecordRepository.Update(zktClockingInRecord);
                _unitOfWork.Save();
            }

            if (zktClockingOutRecord != null)
            {
                zktClockingOutRecord.IsMatched = true;
                _unitOfWork.ZkTimeClockingRecordRepository.Update(zktClockingOutRecord);
                _unitOfWork.Save();
            }
        }
    }
}
