using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.LeaveRequests.Interfaces;
using RMS.AppServiceLayer.LeaveTypes.Enums;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.LeaveRequests.Services
{
    public class LeaveRequestAppService : ILeaveRequestAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public LeaveRequestAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }

        public ICollection<LeaveRequestDto> GetForEmployee(long id)
        {
            var leaveRequests = _unitOfWork.LeaveRequestRepository
                                           .Get(lr => lr.EmployeeId == id &&
                                                      lr.IsActive &&
                                                      lr.IsTaken != true);

            var leaveTypes = _unitOfWork.LeaveTypeRepository.GetAll();

            if (leaveRequests != null)
            {
                var leaveRequestDtos = new List<LeaveRequestDto>();

                foreach (var leaveRequest in leaveRequests)
                {
                    var leaveRequestDto = Mapper.Map<LeaveRequestDto>(leaveRequest);

                    leaveRequestDto.LeaveTypeName = leaveTypes.Where(lt => lt.Id == leaveRequestDto.LeaveTypeId)
                                                              .Select(lt => lt.Name)
                                                              .FirstOrDefault();

                    leaveRequestDtos.Add(leaveRequestDto);
                }

                return leaveRequestDtos;
            }

            return null;
        }

        public ICollection<LeaveRequestDto> GetForIds(List<long> staffIds)
        {
            var leaveRequests = _unitOfWork.LeaveRequestRepository.Get(lr => lr.IsActive && 
                                                                             lr.IsApproved != true && 
                                                                             lr.IsTaken != true)
                                    .Where(lr => staffIds.Contains((long) lr.EmployeeId))
                                    .ToList();

            if (leaveRequests != null)
            {
                var leaveRequestDtos = new List<LeaveRequestDto>();

                foreach (var leaveRequest in leaveRequests)
                {
                    leaveRequestDtos.Add(Mapper.Map<LeaveRequestDto>(leaveRequest));
                }

                return leaveRequestDtos;
            }

            return null;
        }


        // Repo Methods
        public LeaveRequestDto GetById(long id)
        {
            var leaveRequest = _unitOfWork.LeaveRequestRepository.GetById(id);

            if (leaveRequest != null)
            {
                var leaveRequestDto = Mapper.Map<LeaveRequestDto>(leaveRequest);

                return leaveRequestDto;
            }

            return null;
        }

        public ICollection<LeaveRequestDto> GetAll()
        {
            var leaveRequests = _unitOfWork.LeaveRequestRepository.GetAll();

            if (leaveRequests != null)
            {
                var leaveRequestDtos = new List<LeaveRequestDto>();

                foreach (var leaveRequest in leaveRequests)
                {
                    leaveRequestDtos.Add(Mapper.Map<LeaveRequestDto>(leaveRequest));
                }

                return leaveRequestDtos;
            }

            return null;
        }


        // CRUD
        public int Create(LeaveRequestDto leaveRequestDto, long userId)
        {
            if (CheckIfStaffOnShift(leaveRequestDto))
            {
                return (int)StaffOn.Shift;
            }

            if (CheckIfStaffOnAnnual(leaveRequestDto))
            {
                return (int)StaffOn.Annual;
            }

            if (CheckIfStaffOnMaternity(leaveRequestDto))
            {
                return (int)StaffOn.Maternity;
            }

            if (CheckIfStaffOnPaternity(leaveRequestDto))
            {
                return (int)StaffOn.Paternity;
            }

            if (CheckIfStaffOnSick(leaveRequestDto))
            {
                return (int)StaffOn.Sick;
            }

            if (CheckIfStaffOnSuspension(leaveRequestDto))
            {
                return (int)StaffOn.Suspension;
            }

            if (CheckIfStaffOnOther(leaveRequestDto))
            {
                return (int)StaffOn.Other;
            }

            if (CheckIfStaffOnTraining(leaveRequestDto))
            {
                return (int)StaffOn.Training;
            }


            // All good, so go ahead and create
            var leaveRequest = Mapper.Map<LeaveRequest>(leaveRequestDto);

            _unitOfWork.LeaveRequestRepository.Create(leaveRequest);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.LeaveRequestTableName,
                userId,
                leaveRequest.Id);

            // Email Manager for Approval (no need if coming via admin route?)
            //MailerService.SendLeaveRequestEmail(leaveRequestDto);

            return (int)StaffOn.None;
        }

        public int Update(LeaveRequestDto leaveRequestDto, long userId)
        {
            var leaveRequest = _unitOfWork.LeaveRequestRepository.GetById(leaveRequestDto.Id);

            // check start and end date times are the same
            if (!DateTime.Equals(leaveRequest.StartDateTime, leaveRequestDto.StartDateTime) ||
                !DateTime.Equals(leaveRequest.EndDateTime, leaveRequestDto.EndDateTime) ||
                
                (!DateTime.Equals(leaveRequest.StartDateTime, leaveRequestDto.StartDateTime) &&
                 !DateTime.Equals(leaveRequest.EndDateTime, leaveRequestDto.EndDateTime)))
            {
                if (CheckIfStaffOnShift(leaveRequestDto))
                {
                    return (int)StaffOn.Shift;
                }

                if (CheckIfStaffOnAnnual(leaveRequestDto))
                {
                    return (int)StaffOn.Annual;
                }

                if (CheckIfStaffOnMaternity(leaveRequestDto))
                {
                    return (int)StaffOn.Maternity;
                }

                if (CheckIfStaffOnPaternity(leaveRequestDto))
                {
                    return (int)StaffOn.Paternity;
                }

                if (CheckIfStaffOnSick(leaveRequestDto))
                {
                    return (int)StaffOn.Sick;
                }

                if (CheckIfStaffOnSuspension(leaveRequestDto))
                {
                    return (int)StaffOn.Suspension;
                }

                if (CheckIfStaffOnOther(leaveRequestDto))
                {
                    return (int)StaffOn.Other;
                }

                if (CheckIfStaffOnTraining(leaveRequestDto))
                {
                    return (int)StaffOn.Training;
                }
            }

            // All good, so go ahead and update
            CommonHelperAppService.MapDtoToEntityForUpdating(leaveRequestDto, leaveRequest);

            _unitOfWork.LeaveRequestRepository.Update(leaveRequest);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.LeaveRequestTableName,
                userId,
                leaveRequest.Id);

            return (int)StaffOn.None;
        }

        public void Delete(long id, long userId)
        {
            var leaveRequest = _unitOfWork.LeaveRequestRepository.GetById(id);

            leaveRequest.IsActive = CommonHelperAppService.DeleteEntity();
            leaveRequest.IsApproved = false; // this may or may not need to be done. Ask.

            _unitOfWork.LeaveRequestRepository.Update(leaveRequest);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.LeaveRequestTableName,
                userId,
                leaveRequest.Id);
        }


        // Private Methods
        private bool CheckIfStaffOnShift(LeaveRequestDto leaveRequestDto)
        {
            long? shiftStaffId = null;

            // check if staff member is on a shift already
            shiftStaffId = _unitOfWork.ShiftRepository.Get(s => s.EmployeeId == leaveRequestDto.EmployeeId && s.IsActive &&
                (((s.StartDate >= leaveRequestDto.StartDateTime && s.StartDate <= leaveRequestDto.EndDateTime) && (s.EndDate >= leaveRequestDto.EndDateTime)) ||
                 (s.StartDate <= leaveRequestDto.StartDateTime && s.EndDate >= leaveRequestDto.EndDateTime) ||
                 (s.StartDate >= leaveRequestDto.StartDateTime && s.EndDate <= leaveRequestDto.EndDateTime) ||
                ((s.StartDate <= leaveRequestDto.StartDateTime) && (s.EndDate >= leaveRequestDto.StartDateTime && s.EndDate <= leaveRequestDto.EndDateTime))))
            .Select(s => s.EmployeeId)
            .FirstOrDefault();

            if (shiftStaffId != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfStaffOnAnnual(LeaveRequestDto leaveRequestDto)
        {
            long? shiftStaffId = null;

            shiftStaffId = _unitOfWork.LeaveRequestRepository.Get(al => al.EmployeeId == leaveRequestDto.EmployeeId && al.IsActive &&
                (((al.StartDateTime >= leaveRequestDto.StartDateTime && al.StartDateTime <= leaveRequestDto.EndDateTime) && (al.EndDateTime >= leaveRequestDto.EndDateTime)) ||
                 (al.StartDateTime <= leaveRequestDto.StartDateTime && al.EndDateTime >= leaveRequestDto.EndDateTime) ||
                 (al.StartDateTime >= leaveRequestDto.StartDateTime && al.EndDateTime <= leaveRequestDto.EndDateTime) ||
                ((al.StartDateTime <= leaveRequestDto.StartDateTime) && (al.EndDateTime >= leaveRequestDto.StartDateTime && al.EndDateTime <= leaveRequestDto.EndDateTime))))
            .Select(al => al.EmployeeId)
            .FirstOrDefault();

            if (shiftStaffId != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfStaffOnMaternity(LeaveRequestDto leaveRequestDto)
        {
            long? shiftStaffId = null;

            shiftStaffId = _unitOfWork.LeaveRequestRepository.Get(ml => ml.EmployeeId == leaveRequestDto.EmployeeId && ml.IsActive &&
                (((ml.StartDateTime >= leaveRequestDto.StartDateTime && ml.StartDateTime <= leaveRequestDto.EndDateTime) && (ml.EndDateTime >= leaveRequestDto.EndDateTime)) ||
                 (ml.StartDateTime <= leaveRequestDto.StartDateTime && ml.EndDateTime >= leaveRequestDto.EndDateTime) ||
                 (ml.StartDateTime >= leaveRequestDto.StartDateTime && ml.EndDateTime <= leaveRequestDto.EndDateTime) ||
                ((ml.StartDateTime <= leaveRequestDto.StartDateTime) && (ml.EndDateTime >= leaveRequestDto.StartDateTime && ml.EndDateTime <= leaveRequestDto.EndDateTime))))
            .Select(ml => ml.EmployeeId)
            .FirstOrDefault();

            if (shiftStaffId != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfStaffOnPaternity(LeaveRequestDto leaveRequestDto)
        {
            long? shiftStaffId = null;

            shiftStaffId = _unitOfWork.LeaveRequestRepository.Get(pl => pl.EmployeeId == leaveRequestDto.EmployeeId && pl.IsActive &&
                (((pl.StartDateTime >= leaveRequestDto.StartDateTime && pl.StartDateTime <= leaveRequestDto.EndDateTime) && (pl.EndDateTime >= leaveRequestDto.EndDateTime)) ||
                 (pl.StartDateTime <= leaveRequestDto.StartDateTime && pl.EndDateTime >= leaveRequestDto.EndDateTime) ||
                 (pl.StartDateTime >= leaveRequestDto.StartDateTime && pl.EndDateTime <= leaveRequestDto.EndDateTime) ||
                ((pl.StartDateTime <= leaveRequestDto.StartDateTime) && (pl.EndDateTime >= leaveRequestDto.StartDateTime && pl.EndDateTime <= leaveRequestDto.EndDateTime))))
            .Select(pl => pl.EmployeeId)
            .FirstOrDefault();

            if (shiftStaffId != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfStaffOnSick(LeaveRequestDto leaveRequestDto)
        {
            long? shiftStaffId = null;

            shiftStaffId = _unitOfWork.LeaveRequestRepository.Get(sl => sl.EmployeeId == leaveRequestDto.EmployeeId && sl.IsActive &&
                (((sl.StartDateTime >= leaveRequestDto.StartDateTime && sl.StartDateTime <= leaveRequestDto.EndDateTime) && (sl.EndDateTime >= leaveRequestDto.EndDateTime)) ||
                 (sl.StartDateTime <= leaveRequestDto.StartDateTime && sl.EndDateTime >= leaveRequestDto.EndDateTime) ||
                 (sl.StartDateTime >= leaveRequestDto.StartDateTime && sl.EndDateTime <= leaveRequestDto.EndDateTime) ||
                ((sl.StartDateTime <= leaveRequestDto.StartDateTime) && (sl.EndDateTime >= leaveRequestDto.StartDateTime && sl.EndDateTime <= leaveRequestDto.EndDateTime))))
            .Select(sl => sl.EmployeeId)
            .FirstOrDefault();

            if (shiftStaffId != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfStaffOnSuspension(LeaveRequestDto leaveRequestDto)
        {
            long? shiftStaffId = null;

            shiftStaffId = _unitOfWork.LeaveRequestRepository.Get(s => s.EmployeeId == leaveRequestDto.EmployeeId && s.IsActive &&
                (((s.StartDateTime >= leaveRequestDto.StartDateTime && s.StartDateTime <= leaveRequestDto.EndDateTime) && (s.EndDateTime >= leaveRequestDto.EndDateTime)) ||
                 (s.StartDateTime <= leaveRequestDto.StartDateTime && s.EndDateTime >= leaveRequestDto.EndDateTime) ||
                 (s.StartDateTime >= leaveRequestDto.StartDateTime && s.EndDateTime <= leaveRequestDto.EndDateTime) ||
                ((s.StartDateTime <= leaveRequestDto.StartDateTime) && (s.EndDateTime >= leaveRequestDto.StartDateTime && s.EndDateTime <= leaveRequestDto.EndDateTime))))
            .Select(s => s.EmployeeId)
            .FirstOrDefault();

            if (shiftStaffId != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfStaffOnOther(LeaveRequestDto leaveRequestDto)
        {
            long? shiftStaffId = null;

            shiftStaffId = _unitOfWork.LeaveRequestRepository.Get(ol => ol.EmployeeId == leaveRequestDto.EmployeeId && ol.IsActive &&
                (((ol.StartDateTime >= leaveRequestDto.StartDateTime && ol.StartDateTime <= leaveRequestDto.EndDateTime) && (ol.EndDateTime >= leaveRequestDto.EndDateTime)) ||
                 (ol.StartDateTime <= leaveRequestDto.StartDateTime && ol.EndDateTime >= leaveRequestDto.EndDateTime) ||
                 (ol.StartDateTime >= leaveRequestDto.StartDateTime && ol.EndDateTime <= leaveRequestDto.EndDateTime) ||
                ((ol.StartDateTime <= leaveRequestDto.StartDateTime) && (ol.EndDateTime >= leaveRequestDto.StartDateTime && ol.EndDateTime <= leaveRequestDto.EndDateTime))))
            .Select(ol => ol.EmployeeId)
            .FirstOrDefault();

            if (shiftStaffId != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfStaffOnTraining(LeaveRequestDto leaveRequestDto)
        {
            long? shiftStaffId = null;

            shiftStaffId = _unitOfWork.LeaveRequestRepository.Get(t => t.EmployeeId == leaveRequestDto.EmployeeId && t.IsActive &&
                (((t.StartDateTime >= leaveRequestDto.StartDateTime && t.StartDateTime <= leaveRequestDto.EndDateTime) && (t.EndDateTime >= leaveRequestDto.EndDateTime)) ||
                 (t.StartDateTime <= leaveRequestDto.StartDateTime && t.EndDateTime >= leaveRequestDto.EndDateTime) ||
                 (t.StartDateTime >= leaveRequestDto.StartDateTime && t.EndDateTime <= leaveRequestDto.EndDateTime) ||
                ((t.StartDateTime <= leaveRequestDto.StartDateTime) && (t.EndDateTime >= leaveRequestDto.StartDateTime && t.EndDateTime <= leaveRequestDto.EndDateTime))))
            .Select(t => t.EmployeeId)
            .FirstOrDefault();

            if (shiftStaffId != null)
            {
                return true;
            }

            return false;
        }
    }
}
