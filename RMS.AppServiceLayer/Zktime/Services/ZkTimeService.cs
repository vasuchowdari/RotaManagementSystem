using System;
using System.Collections.Generic;
using System.Linq;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Zktime.Dto;
using RMS.AppServiceLayer.Zktime.Enums;
using RMS.AppServiceLayer.Zktime.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;
using RMS.Zktime.Events;
using RMS.Zktime.Interfaces;
using RMS.Zktime.Services;

namespace RMS.AppServiceLayer.Zktime.Services
{
    public class ZkTimeService : IZkTimeService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IZkTimeModule _zkTimeModule;
        private bool _firstRun;
        private DateTime _parameterClockingTime;
        private bool _last24Hrs;
        private long? _employeeId;

        private const double _minusNHours = -2;
        private const int _minusNMonths = -3;

        // this will be filtered down to users setup on both RMS and ZKT
        private ICollection<StaffIdMatrixDto> _staffIdMatrix; 
        private ICollection<Shift> _shifts;
        private ICollection<LeaveRequest> _trainingShifts;
        private ICollection<ShiftProfile> _shiftProfileList;


        public ZkTimeService()
        {
            _zkTimeModule = new ZkTimeModule();
            _zkTimeModule.ZkTimeDataReadyEvent += (sender, args) => MapReturnedDataDirectToEntity(args);
            _zkTimeModule.NoNewZkTimeDataEvent += (sender, args) => GenerateStaffIdNumberMatrix();
            _firstRun = true;

            _parameterClockingTime = DateTime.MinValue;
            _last24Hrs = false;
        }


        //11. UpdateLeaveRequestRecords
        private void UpdateLeaveRequestRecord(
            LeaveRequest tShift,
            DateTime clockInDateTime,
            DateTime clockOutDateTime,
            DateTime actualStartDateTime,
            DateTime actualEndDateTime,
            bool lateIn,
            bool earlyOut,
            bool earlyIn,
            bool missingClockIn,
            bool missingClockOut,
            bool validClockIn,
            bool validClockOut)
        {
            var status = -1;
            var autoApprove = false;

            if (validClockIn && validClockOut) { status = (int)ShiftProfileStatus.Valid; }

            // EARLY IN
            if (earlyIn && validClockOut) { status = (int)ShiftProfileStatus.Valid; }
            if (earlyIn && missingClockOut) { status = (int)ShiftProfileStatus.MissingClockOut; }
            if (earlyIn && earlyOut) { status = (int)ShiftProfileStatus.EarlyOut; }

            // LATE IN
            if (lateIn && validClockOut) { status = (int)ShiftProfileStatus.LateIn; }
            if (lateIn && missingClockOut) { status = (int)ShiftProfileStatus.MissingClockOut; }
            if (lateIn && earlyOut) { status = (int)ShiftProfileStatus.LateIn; }

            // MISSING CLOCK IN
            if (missingClockIn && validClockOut) { status = (int)ShiftProfileStatus.MissingClockIn; }
            if (missingClockIn && missingClockOut) { status = (int)ShiftProfileStatus.NoShow; }
            if (missingClockIn && earlyOut) { status = (int)ShiftProfileStatus.MissingClockIn; }

            // VALID CLOCK IN
            if (validClockIn && missingClockOut) { status = (int)ShiftProfileStatus.MissingClockOut; }
            if (validClockIn && earlyOut) { status = (int)ShiftProfileStatus.EarlyOut; }

            if (status == (int)ShiftProfileStatus.Valid) { autoApprove = true; }


            // Update LeaveRequest
            tShift.Status = status;
            tShift.ActualStartDateTime = actualStartDateTime;
            tShift.ActualEndDateTime = actualEndDateTime;
            tShift.ZktStartDateTime = clockInDateTime;
            tShift.ZktEndDateTime = clockOutDateTime;
            // Is the IsApproved the right one?

            _unitOfWork.LeaveRequestRepository.Update(tShift);
            _unitOfWork.Save();
        }

        //10. ProcessZkTimeDataForTraining
        private void ProcessZkTimeDataForTraining(LeaveRequest tShift, long? employeeZkTimeExtId)
        {
            var listToUpdate = new List<ZkTimeClockingRecord>();
            var employeeDataSet = new List<ZkTimeClockingRecord>();

            if (_last24Hrs)
            {
                _parameterClockingTime = DateTime.Now.AddDays(-1);
            }

            employeeDataSet = _unitOfWork.ZkTimeClockingRecordRepository
                .Get(z => z.ZkTimeBadgeNumber == employeeZkTimeExtId &&
                          z.ClockingTime > _parameterClockingTime &&
                          !z.IsMatched)
                .ToList();

            var lateIn = false;
            var earlyOut = false;
            var earlyIn = false;
            var missingClockIn = false;
            var missingClockOut = false;
            var validClockIn = false;
            var validClockOut = false;

            var trainingStartDateTime = tShift.StartDateTime;
            var trainingEndDateTime = tShift.EndDateTime;

            var trainingPostStartStillValidThreshold = trainingStartDateTime.AddMinutes(5);
            var trainingPreStartEarlyInThreshold = trainingStartDateTime.AddMinutes(-120);
            var trainingPostEndValidThreshold = trainingEndDateTime.AddMinutes(30);

            // UPTO 5 MINUTES AFTER THE SHIFT START TIME
            var clockIn = employeeDataSet.Where(x => x.ClockingTime >= trainingStartDateTime &&
                                                     x.ClockingTime <= trainingPostStartStillValidThreshold)
                                         .Select(x => x)
                                         .FirstOrDefault();

            // UPTO 30 MINUTES AFTER A SHIFT END IS A VALID CLOCK OUT
            var clockOut = employeeDataSet.Where(x => x.ClockingTime >= trainingEndDateTime &&
                                                      x.ClockingTime <= trainingPostEndValidThreshold)
                                          .Select(x => x)
                                          .FirstOrDefault();

            if (clockIn == null)
            {
                // EARLY CLOCK IN IS ANY TIME UPTO 2HRS PRIOR TO SHIFT START
                clockIn = employeeDataSet.Where(x => x.ClockingTime >= trainingPreStartEarlyInThreshold &&
                                                     x.ClockingTime <= trainingStartDateTime)
                                         .Select(x => x)
                                         .FirstOrDefault();



                if (clockIn == null)
                {
                    // LATE CLOCK IN MUST BE ANYTHING AFTER THE 5 MIN GRACE PERIOD AND BEFORE SHIFT END
                    clockIn = employeeDataSet.Where(x => x.ClockingTime > trainingPostStartStillValidThreshold &&
                                                         x.ClockingTime <= trainingEndDateTime)
                                             .Select(x => x)
                                             .FirstOrDefault();

                    if (clockIn == null)
                    {
                        missingClockIn = true;
                    }
                    else
                    {
                        clockIn.TshiftId = tShift.Id;
                        listToUpdate.Add(clockIn);
                        employeeDataSet.Remove(clockIn);
                        lateIn = true;
                    }
                }
                else
                {
                    clockIn.TshiftId = tShift.Id;
                    listToUpdate.Add(clockIn);
                    employeeDataSet.Remove(clockIn);
                    earlyIn = true;
                }
            }
            else
            {
                clockIn.TshiftId = tShift.Id;
                listToUpdate.Add(clockIn);
                employeeDataSet.Remove(clockIn);
                validClockIn = true;
            }

            if (clockOut == null)
            {
                var shiftClockInLateThreshold = trainingStartDateTime.AddHours(2);

                // did they clock out early?
                clockOut = employeeDataSet.Where(x => x.ClockingTime > shiftClockInLateThreshold &&
                                                      x.ClockingTime < trainingEndDateTime)
                                          .Select(x => x)
                                          .FirstOrDefault();

                // compare In & Out. If same, null up Out and continue
                if (clockIn != null && clockIn.Equals(clockOut)) { clockOut = null; }

                if (clockOut == null)
                {
                    missingClockOut = true;
                }
                else
                {
                    clockOut.TshiftId = tShift.Id;
                    listToUpdate.Add(clockOut);
                    employeeDataSet.Remove(clockOut);
                    earlyOut = true;
                }
            }
            else
            {
                clockOut.TshiftId = tShift.Id;
                listToUpdate.Add(clockOut);
                employeeDataSet.Remove(clockOut);
                validClockOut = true;
            }

            var clockInDateTime = ReturnDefaultedDateTime();
            var clockOutDateTime = ReturnDefaultedDateTime();
            var actualStartDateTime = DateTime.Now;
            var actualEndDateTime = DateTime.Now;

            if (clockIn != null)
            {
                clockInDateTime = clockIn.ClockingTime;
                clockIn.IsMatched = true;

                if (clockIn.ClockingTime <= trainingStartDateTime)
                {
                    actualStartDateTime = trainingStartDateTime;
                }

                if (clockIn.ClockingTime > trainingStartDateTime)
                {
                    actualStartDateTime = clockIn.ClockingTime;
                }
            }
            else
            {
                actualStartDateTime = trainingStartDateTime;
            }

            if (clockOut != null)
            {
                clockOutDateTime = clockOut.ClockingTime;
                clockOut.IsMatched = true;

                if (clockOut.ClockingTime <= trainingEndDateTime)
                {
                    actualEndDateTime = clockOut.ClockingTime;
                }

                if (clockOut.ClockingTime > trainingEndDateTime)
                {
                    actualEndDateTime = trainingEndDateTime;
                }
            }
            else
            {
                actualEndDateTime = trainingEndDateTime;
            }

            foreach (var unprocessedZkTimeData in listToUpdate)
            {
                _unitOfWork.ZkTimeClockingRecordRepository.Update(unprocessedZkTimeData);
            }

            _unitOfWork.Save();

            UpdateLeaveRequestRecord(tShift, clockInDateTime, clockOutDateTime,
                actualStartDateTime, actualEndDateTime, lateIn, earlyOut,
                earlyIn, missingClockIn, missingClockOut, validClockIn, validClockOut);
        }

        // 9.
        private void CreateShiftProfileRecords(
            Shift shift,
            DateTime clockInDateTime,
            DateTime clockOutDateTime,
            DateTime actualStartDateTime,
            DateTime actualEndDateTime,
            bool lateIn,
            bool earlyOut,
            bool earlyIn,
            bool missingClockIn,
            bool missingClockOut,
            bool validClockIn,
            bool validClockOut)
        {
            var status = -1;
            var autoApprove = false;

            if (validClockIn && validClockOut) { status = (int)ShiftProfileStatus.Valid; }

            // EARLY IN
            if (earlyIn && validClockOut) { status = (int)ShiftProfileStatus.Valid; }
            //if (earlyIn && validClockOut) { status = (int)ShiftProfileStatus.EarlyIn; }
            if (earlyIn && missingClockOut) { status = (int)ShiftProfileStatus.MissingClockOut; }
            if (earlyIn && earlyOut) { status = (int)ShiftProfileStatus.EarlyOut; }

            // LATE IN
            if (lateIn && validClockOut) { status = (int)ShiftProfileStatus.LateIn; }
            if (lateIn && missingClockOut) { status = (int)ShiftProfileStatus.MissingClockOut; }
            if (lateIn && earlyOut) { status = (int)ShiftProfileStatus.LateIn; }

            // MISSING CLOCK IN
            if (missingClockIn && validClockOut) { status = (int)ShiftProfileStatus.MissingClockIn; }
            if (missingClockIn && missingClockOut) { status = (int)ShiftProfileStatus.NoShow; }
            if (missingClockIn && earlyOut) { status = (int)ShiftProfileStatus.MissingClockIn; }

            // VALID CLOCK IN
            if (validClockIn && missingClockOut) { status = (int)ShiftProfileStatus.MissingClockOut; }
            if (validClockIn && earlyOut) { status = (int)ShiftProfileStatus.EarlyOut; }

            if (status == (int)ShiftProfileStatus.Valid) { autoApprove = true; }
            //if (status == (int)ShiftProfileStatus.EarlyIn) { autoApprove = true; }

            var shiftProfile = new ShiftProfile
            {
                ActualStartDateTime = actualStartDateTime,
                ActualEndDateTime = actualEndDateTime,
                ZktStartDateTime = clockInDateTime,
                ZktEndDateTime = clockOutDateTime,
                EmployeeId = _employeeId,
                EndDateTime = shift.EndDate,
                HoursWorked = CommonHelperAppService.ReturnCalculatedTimespanBetweenTwoDateTimeObjects(actualStartDateTime, actualEndDateTime, status),
                IsActive = true,
                IsApproved = autoApprove,
                ShiftId = shift.Id,
                StartDateTime = shift.StartDate,
                Status = status,
                IsModified = false
            };

            _shiftProfileList.Add(shiftProfile);
        }

        // 8.
        private void ProcessZkTimeData(Shift shift, long? employeeZkTimeExtId)
        {
            var listToUpdate = new List<ZkTimeClockingRecord>();
            var employeeDataSet = new List<ZkTimeClockingRecord>();

            if (_last24Hrs)
            {
                _parameterClockingTime = DateTime.Now.AddDays(-1);
            }

            employeeDataSet = _unitOfWork.ZkTimeClockingRecordRepository
                .Get(z => z.ZkTimeBadgeNumber == employeeZkTimeExtId &&
                          z.ClockingTime > _parameterClockingTime &&
                          !z.IsMatched)
                .ToList();

            var lateIn = false;
            var earlyOut = false;
            var earlyIn = false;
            var missingClockIn = false;
            var missingClockOut = false;
            var validClockIn = false;
            var validClockOut = false;

            var shiftStartDateTime = shift.StartDate;
            var shiftEndDateTime = shift.EndDate;

            var shiftPostStartStillValidThreshold = shiftStartDateTime.AddMinutes(5);
            var shiftPreStartEarlyInThreshold = shiftStartDateTime.AddMinutes(-120);
            var shiftPostEndValidThreshold = shiftEndDateTime.AddMinutes(30);

            // UPTO 5 MINUTES AFTER THE SHIFT START TIME
            var clockIn = employeeDataSet.Where(x => x.ClockingTime >= shiftStartDateTime &&
                                                     x.ClockingTime <= shiftPostStartStillValidThreshold)
                                         .Select(x => x)
                                         .FirstOrDefault();

            // UPTO 30 MINUTES AFTER A SHIFT END IS A VALID CLOCK OUT
            var clockOut = employeeDataSet.Where(x => x.ClockingTime >= shiftEndDateTime &&
                                                      x.ClockingTime <= shiftPostEndValidThreshold)
                                          .Select(x => x)
                                          .FirstOrDefault();

            if (clockIn == null)
            {
                // EARLY CLOCK IN IS ANY TIME UPTO 2HRS PRIOR TO SHIFT START
                clockIn = employeeDataSet.Where(x => x.ClockingTime >= shiftPreStartEarlyInThreshold &&
                                                     x.ClockingTime <= shiftStartDateTime)
                                         .Select(x => x)
                                         .FirstOrDefault();



                if (clockIn == null)
                {
                    // LATE CLOCK IN MUST BE ANYTHING AFTER THE 5 MIN GRACE PERIOD AND BEFORE SHIFT END
                    clockIn = employeeDataSet.Where(x => x.ClockingTime > shiftPostStartStillValidThreshold &&
                                                         x.ClockingTime <= shiftEndDateTime)
                                             .Select(x => x)
                                             .FirstOrDefault();                    

                    if (clockIn == null)
                    {
                        missingClockIn = true;
                    }
                    else
                    {
                        clockIn.ShiftId = shift.Id;
                        listToUpdate.Add(clockIn);
                        employeeDataSet.Remove(clockIn);
                        lateIn = true;
                    }
                }
                else
                {
                    clockIn.ShiftId = shift.Id;
                    listToUpdate.Add(clockIn);
                    employeeDataSet.Remove(clockIn);
                    earlyIn = true;
                }
            }
            else
            {
                clockIn.ShiftId = shift.Id;
                listToUpdate.Add(clockIn);
                employeeDataSet.Remove(clockIn);
                validClockIn = true;
            }

            if (clockOut == null)
            {
                var shiftClockInLateThreshold = shiftStartDateTime.AddHours(2);

                // did they clock out early?
                clockOut = employeeDataSet.Where(x => x.ClockingTime > shiftClockInLateThreshold &&
                                                      x.ClockingTime < shiftEndDateTime)
                                          .Select(x => x)
                                          .FirstOrDefault();

                // compare In & Out. If same, null up Out and continue
                if (clockIn != null && clockIn.Equals(clockOut)) { clockOut = null; }

                if (clockOut == null)
                {
                    missingClockOut = true;
                }
                else
                {
                    clockOut.ShiftId = shift.Id;
                    listToUpdate.Add(clockOut);
                    employeeDataSet.Remove(clockOut);
                    earlyOut = true;
                }
            }
            else
            {
                clockOut.ShiftId = shift.Id;
                listToUpdate.Add(clockOut);
                employeeDataSet.Remove(clockOut);
                validClockOut = true;
            }


            var clockInDateTime = ReturnDefaultedDateTime();
            var clockOutDateTime = ReturnDefaultedDateTime();
            var actualStartDateTime = DateTime.Now;
            var actualEndDateTime = DateTime.Now;

            if (clockIn != null)
            {
                clockInDateTime = clockIn.ClockingTime;
                clockIn.IsMatched = true;

                if (clockIn.ClockingTime <= shiftStartDateTime)
                {
                    actualStartDateTime = shiftStartDateTime;
                }

                if (clockIn.ClockingTime > shiftStartDateTime)
                {
                    actualStartDateTime = clockIn.ClockingTime;
                }
            }
            else
            {
                actualStartDateTime = shiftStartDateTime;
            }

            if (clockOut != null)
            {
                clockOutDateTime = clockOut.ClockingTime;
                clockOut.IsMatched = true;

                if (clockOut.ClockingTime <= shiftEndDateTime)
                {
                    actualEndDateTime = clockOut.ClockingTime;
                }
                
                if (clockOut.ClockingTime > shiftEndDateTime)
                {
                    actualEndDateTime = shiftEndDateTime;
                }
            }
            else
            {
                actualEndDateTime = shiftEndDateTime;
            }

            foreach (var unprocessedZkTimeData in listToUpdate)
            {
                _unitOfWork.ZkTimeClockingRecordRepository.Update(unprocessedZkTimeData);
            }

            _unitOfWork.Save();

            CreateShiftProfileRecords(shift, clockInDateTime, clockOutDateTime, 
                actualStartDateTime, actualEndDateTime, lateIn, earlyOut, 
                earlyIn, missingClockIn, missingClockOut, validClockIn, validClockOut);
        }

        // 7.
        private void MapZkTimeDataToShiftsForShiftProfiles()
        {
            foreach (var shift in _shifts)
            {
                if (shift.EmployeeId != null)
                {
                    _employeeId = shift.EmployeeId;
                    var employeeZkTimeExtId = _staffIdMatrix.Where(s => s.EmployeeId == _employeeId)
                                                            .Select(s => s.ZkTimeUserId)
                                                            .FirstOrDefault();

                    ProcessZkTimeData(shift, employeeZkTimeExtId);
                }
            }

            foreach (var tShift in _trainingShifts)
            {
                _employeeId = tShift.EmployeeId;
                var employeeZkTimeExtId = _staffIdMatrix.Where(s => s.EmployeeId == _employeeId)
                                                            .Select(s => s.ZkTimeUserId)
                                                            .FirstOrDefault();

                ProcessZkTimeDataForTraining(tShift, employeeZkTimeExtId);
            }

            // finished off so trip flag
            if (!_last24Hrs)
            {
                _last24Hrs = true;
            }

            // save the _shiftProfileList
            _unitOfWork.ShiftProfileRepository.CreateRange(_shiftProfileList);
            _unitOfWork.Save();
        }

        // SUBSEQUENT RUNS WILL CALL THIS AS SHIFT PROFILE DATA IN RMS DB
        // 6b.
        private void GetDeltaShiftsEndingNHoursAgo(DateTime shiftProfileEndDateTime)
        {
            var nHoursAgo = DateTime.Now.AddHours(_minusNHours);

            _shifts = _unitOfWork.ShiftRepository
                        .Get(s => s.EndDate > shiftProfileEndDateTime &&
                                  s.EndDate <= nHoursAgo &&
                                  s.IsActive &&
                                  s.EmployeeId != null,
                                  null,
                                  "ShiftTemplate.ShiftType")
                        .OrderBy(s => s.StartDate)
                        .ToList();

            _trainingShifts = _unitOfWork.LeaveRequestRepository
                                         .Get(lr => lr.EndDateTime <= nHoursAgo &&
                                                    lr.IsApproved &&
                                                    lr.LeaveTypeId == 7 &&
                                                    lr.Status == 9 &&
                                                    !lr.IsTaken &&
                                                    lr.IsActive)
                                         .OrderBy(lr => lr.StartDateTime)
                                         .ToList();

            MapZkTimeDataToShiftsForShiftProfiles();
        }

        // FIRST RUN WILL CALL THIS AS NO SHIFT PROFILE DATA IN RMS DB
        // 6a.
        private void GetHistoricalShiftsEndingNHoursAgo()
        {
            var nHoursAgo = DateTime.Now.AddHours(_minusNHours);
            var nMonthsAgo = DateTime.Now.AddMonths(_minusNMonths);

            _shifts = _unitOfWork.ShiftRepository
                                 .Get(s => s.EndDate <= nHoursAgo &&
                                           //s.StartDate >= nMonthsAgo &&
                                           s.EndDate >= nMonthsAgo &&
                                           s.IsAssigned && 
                                           s.IsActive,
                                           null,
                                           "ShiftTemplate.ShiftType")
                                 .OrderBy(s => s.StartDate)          
                                 .ToList();

            _trainingShifts = _unitOfWork.LeaveRequestRepository
                                         .Get(lr => lr.EndDateTime <= nHoursAgo &&
                                                    lr.EndDateTime >= nMonthsAgo &&
                                                    lr.IsApproved &&
                                                    lr.LeaveTypeId == 7 &&
                                                    lr.Status == 9 &&
                                                    !lr.IsTaken &&
                                                    lr.IsActive)
                                         .OrderBy(lr => lr.StartDateTime)
                                         .ToList();

            MapZkTimeDataToShiftsForShiftProfiles();
        }

        // 5.
        private void VerifyShiftProfileRecordExistence()
        {
            var shiftProfile = _unitOfWork.ShiftProfileRepository
                                          .Get(sp => sp.IsActive && sp.Status != 8)
                                          .OrderByDescending(x => x.EndDateTime)
                                          .FirstOrDefault();

            if (shiftProfile != null)
            {
                // THERE ARE SHIFT PROFILE RECORDS, SO ASCERTAIN LAST ENTRY
                // AND CREATE ANY NEW RECORDS BETWEEN THEN AND (NOW)
                GetDeltaShiftsEndingNHoursAgo(shiftProfile.EndDateTime);
            }
            else
            {
                GetHistoricalShiftsEndingNHoursAgo();
            }
        }

        // 4.
        private void GenerateStaffIdNumberMatrix()
        {
            // flush every time in case new staff have been entered into system since last time
            _staffIdMatrix = new List<StaffIdMatrixDto>();

            var employees = _unitOfWork.EmployeeRepository.Get(e => e.IsActive, null, "User");

            foreach (var employee in employees)
            {
                var matrixEntry = new StaffIdMatrixDto
                {
                    EmployeeId = employee.Id,
                    UserId = employee.User.Id,
                    ZkTimeUserId = employee.User.ExternalTimeSystemId
                };

                _staffIdMatrix.Add(matrixEntry);
            }

            VerifyShiftProfileRecordExistence();
        }

        // 3. 
        private void PersistUnprocessedZkTimeData(IEnumerable<ZkTimeClockingRecord> unprocessedZkTimeDataList)
        {
            _unitOfWork.ZkTimeClockingRecordRepository.CreateRange(unprocessedZkTimeDataList);
            _unitOfWork.Save();

            GenerateStaffIdNumberMatrix();
        }

        // 2.
        private void MapReturnedDataDirectToEntity(ZkTimeDataEventArgs e)
        {
            var unprocessedDataEntityList = e.UserClockingData.Select(rawZkTimeDataObj => new ZkTimeClockingRecord
            {
                IsActive = true, 
                ClockingTime = rawZkTimeDataObj.ClockingTime, 
                ZkTimeBadgeNumber = rawZkTimeDataObj.StaffId, 
                ZkTimeSiteNumber = rawZkTimeDataObj.SiteId, 
                ZkTimeSiteName = rawZkTimeDataObj.SiteName, 
                ZkTimeUserName = rawZkTimeDataObj.StaffName, 
                ShiftId = rawZkTimeDataObj.ShiftId
            }).ToList();

            PersistUnprocessedZkTimeData(unprocessedDataEntityList);
        }

        // 1.
        public void Init()
        {
            var mostRecentEntryClockingTime = DateTime.MinValue;
            var unprocessedRecord = _unitOfWork.ZkTimeClockingRecordRepository.GetAll().LastOrDefault();
            
            _shiftProfileList = new List<ShiftProfile>();

            if (!_firstRun)
            {
                if (unprocessedRecord != null)
                {
                    mostRecentEntryClockingTime = unprocessedRecord.ClockingTime;
                    _zkTimeModule.Init(_firstRun, mostRecentEntryClockingTime);
                }
            }
            else
            {
                if (unprocessedRecord != null)
                {
                    _firstRun = false;
                    mostRecentEntryClockingTime = unprocessedRecord.ClockingTime;
                }

                _zkTimeModule.Init(_firstRun, mostRecentEntryClockingTime);
            }
        }

        // HELPER METHOD
        private DateTime ReturnDefaultedDateTime()
        {
            var defaultedDateTime = DateTime.SpecifyKind(new DateTime(1900, 1, 1), DateTimeKind.Local);

            defaultedDateTime = defaultedDateTime.ToLocalTime();
            return defaultedDateTime;
        }
    }
}