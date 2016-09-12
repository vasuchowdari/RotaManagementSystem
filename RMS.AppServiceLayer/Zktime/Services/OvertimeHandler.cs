using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.ShiftProfiles.Dto;
using RMS.AppServiceLayer.Zktime.Dto;
using RMS.AppServiceLayer.Zktime.Enums;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Zktime.Services
{
    public class OvertimeHandler
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private DateTime _endTimeThresholdMin;
        private DateTime _endTimeThresholdMax;
        private ICollection<StaffIdMatrixDto> _staffIdMatrix;
        private bool _firstRun;
        private const double _minusNHours = -5;
        private const int _minusNMonths = -3;

        public OvertimeHandler()
        {
            _firstRun = true;
        }

        private void MatchAnyLatentClockOuts(IEnumerable<ShiftProfileDto> shiftProfiles, IEnumerable<ZkTimeClockingRecord> zktimeClockingRecords)
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

            foreach (var shiftProfileDto in shiftProfiles)
            {
                if (shiftProfileDto.EmployeeId != null)
                {
                    var matchedId =
                        _staffIdMatrix.Where(sm => sm.EmployeeId == shiftProfileDto.EmployeeId)
                                      .Select(sm => sm.ZkTimeUserId)              
                                      .FirstOrDefault();

                    // shift end time + 5hrs
                    var newEndDateTime = shiftProfileDto.EndDateTime.AddHours(5);

                    var result = zktimeClockingRecords.FirstOrDefault(z => z.ZkTimeBadgeNumber == matchedId &&
                                                                           z.ClockingTime >= shiftProfileDto.EndDateTime &&
                                                                           z.ClockingTime <= newEndDateTime);

                    if (result != null)
                    {
                        if (result.ClockingTime < shiftProfileDto.EndDateTime)
                        {
                            return;
                        }

                        // update existing record
                        shiftProfileDto.Status = (int)ShiftProfileStatus.Valid;

                        shiftProfileDto.ActualEndDateTime = shiftProfileDto.EndDateTime;
                        shiftProfileDto.ZktEndDateTime = shiftProfileDto.EndDateTime;
                        
                        shiftProfileDto.HoursWorked = CommonHelperAppService.ReturnCalculatedTimespanBetweenTwoDateTimeObjects(shiftProfileDto.StartDateTime, shiftProfileDto.EndDateTime, shiftProfileDto.Status);
                        shiftProfileDto.IsApproved = true;

                        var shiftProfile = _unitOfWork.ShiftProfileRepository.GetById(shiftProfileDto.Id);
                        CommonHelperAppService.MapDtoToEntityForUpdating(shiftProfileDto, shiftProfile);
                        _unitOfWork.ShiftProfileRepository.Update(shiftProfile);
                        _unitOfWork.Save();

                        // create the overtime shift profile record
                        var overstayShiftProfile = new ShiftProfile
                        {
                            ActualStartDateTime = shiftProfileDto.EndDateTime,
                            ActualEndDateTime = result.ClockingTime,
                            ZktStartDateTime = shiftProfileDto.EndDateTime,
                            ZktEndDateTime = result.ClockingTime,
                            EmployeeId = shiftProfileDto.EmployeeId,
                            EndDateTime = result.ClockingTime,
                            HoursWorked = CommonHelperAppService.ReturnCalculatedTimespanBetweenTwoDateTimeObjects(shiftProfileDto.EndDateTime, result.ClockingTime, (int)ShiftProfileStatus.OverStayed),
                            IsActive = true,
                            IsApproved = false,
                            ShiftId = shiftProfileDto.ShiftId,
                            StartDateTime = shiftProfileDto.EndDateTime,
                            Status = (int)ShiftProfileStatus.OverStayed,
                            IsModified = false
                        };

                        _unitOfWork.ShiftProfileRepository.Create(overstayShiftProfile);
                        _unitOfWork.Save();

                        result.ShiftId = shiftProfile.ShiftId;
                        result.IsMatched = true;
                        _unitOfWork.ZkTimeClockingRecordRepository.Update(result);
                        _unitOfWork.Save();
                    }
                    // else it's still missing a clock out so leave alone
                }
            }
        }

        private IEnumerable<ZkTimeClockingRecord> GetUnprocessedZktDataForTimePeriod()
        {
            var unprocZktDataList = _unitOfWork.ZkTimeClockingRecordRepository
                .Get(z => z.ClockingTime >= _endTimeThresholdMin &&
                          z.ClockingTime <= _endTimeThresholdMax &&
                          z.ShiftId == null &&
                          !z.IsMatched)
                .OrderByDescending(u => u.ClockingTime)
                .ToList();

            return unprocZktDataList;
        }

        private IEnumerable<ShiftProfileDto> GetAllMissingClockOutShiftProfileRecords()
        {
            if (!_firstRun)
            {
                _endTimeThresholdMin = DateTime.Now.AddHours(_minusNHours);
            }
            else
            {
                _firstRun = false;
                _endTimeThresholdMin = DateTime.Now.AddMonths(_minusNMonths);
            }
            _endTimeThresholdMax = DateTime.Now;

            var mcoShiftProfileList = _unitOfWork.ShiftProfileRepository
                .Get(sp => sp.Status == 6 &&
                           sp.EndDateTime >= _endTimeThresholdMin &&
                           sp.EndDateTime <= _endTimeThresholdMax)
                .ToList();

            var shiftProfileDtoList = new List<ShiftProfileDto>();
            foreach (var shiftProfile in mcoShiftProfileList)
            {
                var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfile);

                shiftProfileDtoList.Add(shiftProfileDto);
            }

            return shiftProfileDtoList;
        }

        public void Init()
        {
            // Get MCO Shift Profile
            var shiftProfiles = GetAllMissingClockOutShiftProfileRecords();

            // Get Unprocessed ZKT Data
            var unprocZktData = GetUnprocessedZktDataForTimePeriod();

            MatchAnyLatentClockOuts(shiftProfiles, unprocZktData);
        }
    }
}
