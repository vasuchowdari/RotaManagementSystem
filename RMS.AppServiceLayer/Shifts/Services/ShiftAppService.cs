using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Helpers.Dto;
using RMS.AppServiceLayer.Helpers.Services;
using RMS.AppServiceLayer.Shifts.Dto;
using RMS.AppServiceLayer.Shifts.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Shifts.Services
{
    public class ShiftAppService : IShiftAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ShiftAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }


        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }

        public ICollection<long> CheckIfStaffAssignedToShiftByDateRange(ICollection<long> staffIds, DateTime shiftRangeStartDate, DateTime shiftRangeEndDate)
        {
            var staffToRemove = new List<long>();
            long? shiftStaffId = null;

            foreach (var staffId in staffIds)
            {
                shiftStaffId = _unitOfWork.ShiftRepository.Get(s => s.EmployeeId == staffId && s.IsActive &&
                                                                    ((s.StartDate >= shiftRangeStartDate && s.StartDate <= shiftRangeEndDate) ||
                                                                     (s.EndDate >= shiftRangeStartDate && s.EndDate <= shiftRangeEndDate)))
                                                          .Select(s => s.EmployeeId)
                                                          .FirstOrDefault();

                if (shiftStaffId != null)
                {
                    staffToRemove.Add(staffId);
                }
            }

            return staffToRemove;
        }

        public string GetSiteName(long shiftId)
        {
            var shiftTemplateId = _unitOfWork.ShiftRepository.GetById(shiftId).ShiftTemplateId;
            var shiftTemplate = _unitOfWork.ShiftTemplateRepository.GetById(shiftTemplateId);
            var name = string.Empty;

            if (shiftTemplate.SubSiteId != null)
            {
                name = _unitOfWork.SubSiteRepository.GetById(shiftTemplate.SubSiteId).Name;
                return name;
            }

            if (shiftTemplate.SiteId != null)
            {
                name = _unitOfWork.SiteRepository.GetById(shiftTemplate.SiteId).Name;
            }

            return name;
        }

        public bool CheckIfShiftExistsForResourceReq(ShiftDto shiftDto)
        {
            var existingShift = _unitOfWork.ShiftRepository.Get(s => s.IsActive &&
                                                                ((s.StartDate == shiftDto.StartDate && s.EndDate == shiftDto.EndDate) ||
                                                                (s.StartDate <= shiftDto.StartDate && s.EndDate >= shiftDto.EndDate)) &&
                                                                s.CalendarResourceRequirementId == shiftDto.CalendarResourceRequirementId)
                                                           .FirstOrDefault();

            if (existingShift != null)
            {
                return true;
            }

            return false;
        }



        // Repo Methods
        public ShiftDto GetById(long id)
        {
            var shift = _unitOfWork.ShiftRepository.Get(s => s.Id == id && s.IsActive,
                                                        null,
                                                        "ShiftTemplate")
                                                   .FirstOrDefault();

            if (shift != null)
            {
                var shiftDto = Mapper.Map<ShiftDto>(shift);

                return shiftDto;
            }

            return null;
        }

        public ICollection<ShiftDto> GetAll()
        {
            var shifts = _unitOfWork.ShiftRepository.GetAll();

            if (shifts != null)
            {
                var shiftDtos = new List<ShiftDto>();

                foreach (var shift in shifts)
                {
                    shiftDtos.Add(Mapper.Map<ShiftDto>(shift));
                }

                return shiftDtos;
            }

            return null;
        }

        public ICollection<ShiftDto> GetByEmployeeId(long id)
        {
            var shifts = _unitOfWork.ShiftRepository.Get(s => s.EmployeeId == id &&
                                                              s.IsActive &&
                                                              s.IsAssigned &&
                                                              s.StartDate > DateTime.Now)
                                                    .ToList();

            if (shifts != null)
            {
                var shiftDtos = new List<ShiftDto>();

                foreach (var shift in shifts)
                {
                    var shiftDto = Mapper.Map<ShiftDto>(shift);

                    var shiftTemplate = _unitOfWork.ShiftTemplateRepository.GetById(shiftDto.ShiftTemplateId);
                    if (shiftTemplate.SubSiteId != null)
                    {
                        shiftDto.Location =
                            _unitOfWork.SubSiteRepository.Get(ss => ss.Id == shiftTemplate.SubSiteId && ss.IsActive)
                                .Select(ss => ss.Name)
                                .FirstOrDefault();
                    }
                    else
                    {
                        shiftDto.Location =
                            _unitOfWork.SiteRepository.Get(s => s.Id == shiftTemplate.SubSiteId && s.IsActive)
                                .Select(s => s.Name)
                                .FirstOrDefault();
                    }

                    shiftDtos.Add(shiftDto);
                }

                return shiftDtos;
            }

            return null;
        }

        public ICollection<ShiftDto> GetForCalendarResourceRequirment(long calResRqId, DateTime startDate, DateTime endDate)
        {
            var shifts = _unitOfWork.ShiftRepository.Get(s => s.IsActive &&
                                                              s.EndDate >= startDate &&
                                                              s.StartDate <= endDate &&
                                                              s.CalendarResourceRequirementId == calResRqId,
                                                              null,
                                                              "ShiftTemplate")
                                                    .ToList();

            var shiftDtos = new List<ShiftDto>();

            foreach (var shift in shifts)
            {
                shiftDtos.Add(Mapper.Map<ShiftDto>(shift));
            }

            return shiftDtos;
        } 


        // CRUD
        public void Create(ShiftDto shiftDto, long userId)
        {
            var shift = Mapper.Map<Shift>(shiftDto);

            _unitOfWork.ShiftRepository.Create(shift);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.ShiftTableName,
                userId,
                shift.Id);

            // TEMP FOR BALDOCK
            //SendTempEmailToMaddie(shiftDto);
        }

        public void Update(ShiftDto shiftDto, long userId)
        {
            var shift = _unitOfWork.ShiftRepository.GetById(shiftDto.Id);

            if (shift.StartDate > DateTime.Now)
            {
                CommonHelperAppService.MapDtoToEntityForUpdating(shiftDto, shift);

                _unitOfWork.ShiftRepository.Update(shift);
                _unitOfWork.Save();

                // Audit
                _auditLogAppService.Audit(
                    AppConstants.ActionTypeUpdate,
                    AppConstants.ShiftTableName,
                    userId,
                    shift.Id);

                // TEMP FOR BALDOCK
                SendTempEmailToMaddie(shiftDto);   
            }
        }

        public void Delete(long id, long userId)
        {
            var shift = _unitOfWork.ShiftRepository.GetById(id);

            if (shift.StartDate > DateTime.Now)
            {
                //shift.IsActive = CommonHelperAppService.DeleteEntity();

                _unitOfWork.ShiftRepository.Delete(shift);
                _unitOfWork.Save();

                // Audit
                _auditLogAppService.Audit(
                    AppConstants.ActionTypeDelete,
                    AppConstants.ShiftTableName,
                    userId,
                    shift.Id);    
            }
        }

        private void SendTempEmailToMaddie(ShiftDto shiftDto)
        {
            // TEMP BITS FOR BALDOCK ###################################################################################
            var mosName = string.Empty;

            // get user details from employeeId
            if (shiftDto.EmployeeId != null)
            {
                var employee = _unitOfWork.EmployeeRepository.GetById(shiftDto.EmployeeId);
                employee.User = _unitOfWork.UserRepository.GetById(employee.UserId);
                mosName = employee.User.Firstname + " " + employee.User.Lastname;
            }

            var tempShiftTemplate = _unitOfWork.ShiftTemplateRepository.GetById(shiftDto.ShiftTemplateId);
            var tempSite = _unitOfWork.SiteRepository.GetById(tempShiftTemplate.SiteId);
            var siteName = tempSite.Name;

            if (tempShiftTemplate.SubSiteId != null)
            {
                var tempSubSite = _unitOfWork.SubSiteRepository.GetById(tempShiftTemplate.SubSiteId);
                siteName = tempSubSite.Name;
            }

            var tempEmailDto = new TempMaddieEmailDto
            {
                ShiftEndDateTime = shiftDto.EndDate.ToString(),
                ShiftStartDateTime = shiftDto.StartDate.ToString(),
                ShiftLocation = siteName,
                ShiftNewStaffMember = mosName,
                ShiftOldStaffMember = shiftDto.TempCurrentStaffMember,
                ResourceTypeName = shiftDto.TempResourceTypeName
            };

            // TEMP EMAIL - send details about shift and changes from here
            MailerService.SendTempMaddieEmail(tempEmailDto);
            // ########################################################################################################
        }
    }
}