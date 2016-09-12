using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.ShiftProfiles.Dto;
using RMS.AppServiceLayer.ShiftProfiles.Interfaces;
using RMS.AppServiceLayer.Zktime.Enums;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.ShiftProfiles.Services
{
    public class ShiftProfileAppService : IShiftProfileAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ShiftProfileAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }

        public int ConsecutiveDayCalculatorDefault(long id, bool isEmployee)
        {
            var shiftProfileList = new List<ShiftProfile>();
            shiftProfileList.AddRange(_unitOfWork.ShiftProfileRepository.Get(sp => sp.EmployeeId == id));

            var counter = 0;
            var today = DateTime.Now;

            // filter the result against today value to get
            // shifts only in the last 2 weeks (13 days)
            var dateFilteredResults = shiftProfileList.Where(
                sp => sp.StartDateTime.Date <= today.Date &&
                      sp.StartDateTime.Date >= today.AddDays(-13).Date &&
                      sp.IsActive);

            var orderedList = dateFilteredResults.OrderByDescending(sp => sp.StartDateTime);
            var lastShift = new DateTime();

            foreach (var shiftProfile in orderedList)
            {
                // no previous shift first time through
                if (counter > 0)
                {
                    var result = (lastShift - shiftProfile.StartDateTime).TotalDays;
                    if (result > 1)
                    {
                        // break in consecutive days
                        break;
                    }

                    lastShift = shiftProfile.StartDateTime;
                }
                else
                {
                    lastShift = shiftProfile.StartDateTime;

                    var result = (today.Date - lastShift.Date).TotalDays;
                    if (result > 1)
                    {
                        // break in consecutive days
                        break;
                    }
                }

                counter++;
            }

            return counter;
        }

        public int ConsecutiveDayCalculator(long id, bool isEmployee, DateTime startDate, DateTime endDate)
        {
            var shiftProfileList = new List<ShiftProfile>();
            shiftProfileList.AddRange(_unitOfWork.ShiftProfileRepository.Get(sp => sp.EmployeeId == id));

            var counter = 0;

            var dateFilteredResults = shiftProfileList.Where(
                sp => sp.StartDateTime.Date <= startDate.Date &&
                      sp.StartDateTime.Date >= startDate.AddDays(-13).Date &&
                      sp.IsActive);

            var orderedList = dateFilteredResults.OrderByDescending(sp => sp.StartDateTime);
            var lastShift = new DateTime();


            foreach (var shiftProfile in orderedList)
            {
                // no previous shift first time through
                if (counter > 0)
                {
                    var result = (lastShift - shiftProfile.StartDateTime).TotalDays;
                    if (result > 1)
                    {
                        // break in consecutive days
                        break;
                    }

                    lastShift = shiftProfile.StartDateTime;
                }
                else
                {
                    lastShift = shiftProfile.StartDateTime;

                    var result = (startDate.Date - lastShift.Date).TotalDays;
                    if (result > 1)
                    {
                        // break in consecutive days
                        break;
                    }
                }

                counter++;
            }

            return counter;
        }

        public ICollection<ShiftProfileDto> ReturnOrderedShiftProfiles()
        {
            var shiftProfileDtos = new List<ShiftProfileDto>();
            var shiftProfiles = _unitOfWork.ShiftProfileRepository.GetAll()
                                           .OrderBy(sp => sp.StartDateTime);

            if (shiftProfiles != null)
            {
                foreach (var shiftProfile in shiftProfiles)
                {
                    var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfile);
                    shiftProfileDto.TimeWorked = CommonHelperAppService.ReturnConvertedTicksAsTimespan(shiftProfileDto.HoursWorked);
                    shiftProfileDtos.Add(shiftProfileDto);
                }

                return shiftProfileDtos;
            }

            return null;
        }

        public List<ShiftProfileDto> GetInvalidShiftProfiles()
        {
            //var dataToReturn = new List<ShiftProfileDto>();
            var shiftProfiles = _unitOfWork.ShiftProfileRepository
                                           .Get(sp => sp.IsActive &&
                                                      sp.IsApproved == false &&
                                                      sp.Status != 0,
                                                      null,
                                                      "Shift, Shift.ShiftTemplate")
                                           .OrderBy(sp => sp.StartDateTime);

            if (shiftProfiles != null)
            {
                var shiftProfileDtos = new List<ShiftProfileDto>();

                foreach (var shiftProfile in shiftProfiles)
                {
                    var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfile);
                    shiftProfileDto.TimeWorked = CommonHelperAppService.ReturnConvertedTicksAsTimespan(shiftProfileDto.HoursWorked);
                    shiftProfileDtos.Add(shiftProfileDto);
                }

                return shiftProfileDtos;
            }

            //if (shiftProfiles != null)
            //{
            //    var groupedShiftProfiles = shiftProfiles.GroupBy(sp => sp.ShiftId);

            //    foreach (var groupedShiftProfile in groupedShiftProfiles)
            //    {
            //        var shiftProfileDtos = new List<ShiftProfileDto>();

            //        foreach (var shiftProfile in groupedShiftProfile)
            //        {
            //            var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfile);
            //            shiftProfileDto.TimeWorked = CommonHelperAppService.ReturnConvertedTicksAsTimespan(shiftProfileDto.HoursWorked);
            //            shiftProfileDtos.Add(shiftProfileDto);
            //        }

            //        dataToReturn.Add(shiftProfileDtos);
            //    }

            //    return dataToReturn;
            //}

            return null;
        }

        public List<ShiftProfileDto> GetInvalidForEmployee(long employeeId)
        {
            var shiftProfileDtos = new List<ShiftProfileDto>();

            var shiftProfiles = _unitOfWork.ShiftProfileRepository
                                           .Get(sp => sp.IsActive &&
                                                      sp.IsApproved == false &&
                                                      sp.IsModified == false &&
                                                      sp.EmployeeId == employeeId &&
                                                      sp.Status != 0)
                                           .OrderBy(sp => sp.StartDateTime);

            foreach (var shiftProfile in shiftProfiles)
            {
                var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfile);
                shiftProfileDto.TimeWorked = CommonHelperAppService.ReturnConvertedTicksAsTimespan(shiftProfileDto.HoursWorked);
                shiftProfileDtos.Add(shiftProfileDto);
            }

            return shiftProfileDtos;
        }

        public ICollection<ShiftProfileDto> GetAllForShift(long id)
        {
            var shiftProfiles = _unitOfWork.ShiftProfileRepository
                                           .Get(sp => sp.ShiftId == id && sp.IsActive);

            if (shiftProfiles != null)
            {
                var shiftProfileDtos = new List<ShiftProfileDto>();

                foreach (var shiftProfile in shiftProfiles)
                {
                    shiftProfileDtos.Add(Mapper.Map<ShiftProfileDto>(shiftProfile));
                }

                return shiftProfileDtos;
            }

            return null;
        }

        public ShiftProfileDto CheckApproval(long id)
        {
            var shiftProfile = _unitOfWork.ShiftProfileRepository
                .Get(sp => sp.ShiftId == id &&
                           sp.IsApproved &&
                           sp.IsActive)
                .FirstOrDefault();

            if (shiftProfile != null)
            {
                var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfile);

                return shiftProfileDto;
            }

            return null;
        }

        // Repo Methods
        public ShiftProfileDto GetById(long id)
        {
            var shiftProfile = _unitOfWork.ShiftProfileRepository.GetById(id);

            if (shiftProfile != null)
            {
                var shiftProfileDto = Mapper.Map<ShiftProfileDto>(shiftProfile);

                return shiftProfileDto;
            }

            return null;
        }

        public ICollection<ShiftProfileDto> GetAll()
        {
            var shiftProfiles = _unitOfWork.ShiftProfileRepository.GetAll();

            if (shiftProfiles != null)
            {
                var shiftProfileDtos = new List<ShiftProfileDto>();

                foreach (var shiftProfile in shiftProfiles)
                {
                    shiftProfileDtos.Add(Mapper.Map<ShiftProfileDto>(shiftProfile));
                }

                return shiftProfileDtos;
            }

            return null;
        }


        // CRUD
        public void Create(ShiftProfileDto shiftProfileDto, long userId)
        {
            shiftProfileDto.HoursWorked =
                CommonHelperAppService.ReturnCalculatedTimespanBetweenTwoDateTimeObjects(
                    shiftProfileDto.ActualStartDateTime, shiftProfileDto.ActualEndDateTime, shiftProfileDto.Status);

            var shiftProfile = Mapper.Map<ShiftProfile>(shiftProfileDto);

            _unitOfWork.ShiftProfileRepository.Create(shiftProfile);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.ShiftProfileTableName,
                userId,
                shiftProfile.Id);

            UpdateAndDeleteNoShow(shiftProfileDto.ShiftId, userId);
        }

        public void Update(ShiftProfileDto shiftProfileDto, long userId)
        {
            var shiftProfile = _unitOfWork.ShiftProfileRepository.GetById(shiftProfileDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(shiftProfileDto, shiftProfile);

            _unitOfWork.ShiftProfileRepository.Update(shiftProfile);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.ShiftProfileTableName,
                userId,
                shiftProfile.Id);
        }

        public void Delete(long id, long userId)
        {
            var shiftProfile = _unitOfWork.ShiftProfileRepository.GetById(id);

            shiftProfile.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.ShiftProfileRepository.Update(shiftProfile);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.ShiftProfileTableName,
                userId,
                shiftProfile.Id);
        }


        private void UpdateAndDeleteNoShow(long shiftId, long userId)
        {
            var shiftProfile = _unitOfWork.ShiftProfileRepository.Get(sp => sp.ShiftId == shiftId &&
                                                                            sp.IsActive)
                                                                 .FirstOrDefault();

            if (shiftProfile != null)
            {
                shiftProfile.IsModified = true;
                shiftProfile.IsActive = false;

                _unitOfWork.ShiftProfileRepository.Update(shiftProfile);
                _unitOfWork.Save();

                // Audit
                _auditLogAppService.Audit(
                    AppConstants.ActionTypeDelete,
                    AppConstants.ShiftProfileTableName,
                    userId,
                    shiftProfile.Id);
            }
        }
    }
}
