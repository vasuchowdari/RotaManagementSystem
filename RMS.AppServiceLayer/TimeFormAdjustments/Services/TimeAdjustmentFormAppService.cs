using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.TimeFormAdjustments.Dto;
using RMS.AppServiceLayer.TimeFormAdjustments.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.TimeFormAdjustments.Services
{
    public class TimeAdjustmentFormAppService : ITimeAdjustmentFormAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public TimeAdjustmentFormAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }

        public ICollection<TimeAdjustmentFormDto> GetForIds(List<long> staffIds)
        {
            var timeAdjustmentForms = _unitOfWork.TimeAdjustmentFormRepository.Get(taf => taf.IsActive && taf.IsManagerApproved == false)
                                                                              .Where(taf => staffIds.Contains((long) taf.EmployeeId))
                                                                              .ToList();

            if (timeAdjustmentForms != null)
            {
                var timeAdjustmentFormDtos = new List<TimeAdjustmentFormDto>();

                foreach (var timeAdjustmentForm in timeAdjustmentForms)
                {
                    timeAdjustmentFormDtos.Add(Mapper.Map<TimeAdjustmentFormDto>(timeAdjustmentForm));
                }

                return timeAdjustmentFormDtos;
            }

            return null;
        }

        public ICollection<TimeAdjustmentFormDto> GetAllUnapproved()
        {
            var timeAdjustmentFormDtos = new List<TimeAdjustmentFormDto>();
            var timeAdjustmentForms = 
                _unitOfWork.TimeAdjustmentFormRepository.Get(taf => taf.IsActive &&
                                                                    taf.IsManagerApproved &&
                                                                    taf.IsAdminApproved == false)
                                                        .ToList();

            if (timeAdjustmentForms.Count > 0)
            {
                foreach (var timeAdjustmentForm in timeAdjustmentForms)
                {
                    timeAdjustmentFormDtos.Add(Mapper.Map<TimeAdjustmentFormDto>(timeAdjustmentForm));
                }
            }

            return timeAdjustmentFormDtos;
        }

        public TimeAdjustmentFormDto GetById(long id)
        {
            var timeAdjustmentForm = _unitOfWork.TimeAdjustmentFormRepository.GetById(id);

            if (timeAdjustmentForm != null)
            {
                var timeAdjustmentFormDto = Mapper.Map<TimeAdjustmentFormDto>(timeAdjustmentForm);

                return timeAdjustmentFormDto;
            }

            return null;
        }

        public ICollection<TimeAdjustmentFormDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<TimeAdjustmentFormDto> GetForEmployee(long id)
        {
            throw new NotImplementedException();
        }

        public ICollection<TimeAdjustmentFormDto> GetForUserId(long id)
        {
            throw new NotImplementedException();
        }

        public ICollection<TimeAdjustmentFormDto> GetForSite(long id)
        {
            throw new NotImplementedException();
        }

        public ICollection<TimeAdjustmentFormDto> GetForSubSite(long id)
        {
            throw new NotImplementedException();
        }

        public void Create(TimeAdjustmentFormDto timeAdjustmentFormDto, long userId)
        {
            var timeAdjustmentForm = Mapper.Map<TimeAdjustmentForm>(timeAdjustmentFormDto);

            _unitOfWork.TimeAdjustmentFormRepository.Create(timeAdjustmentForm);
            _unitOfWork.Save();

            // trip IsModified flag to remove from User Dash
            var shiftProfile = _unitOfWork.ShiftProfileRepository.GetById(timeAdjustmentFormDto.ShiftProfileId);
            shiftProfile.IsModified = true;
            _unitOfWork.ShiftProfileRepository.Update(shiftProfile);
            _unitOfWork.Save();


            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.TimeAdjustmentFormTableName,
                userId,
                timeAdjustmentForm.Id);
        }

        public void Update(TimeAdjustmentFormDto timeAdjustmentFormDto, long userId)
        {
            var timeAdjustmentForm = _unitOfWork.TimeAdjustmentFormRepository.GetById(timeAdjustmentFormDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(timeAdjustmentFormDto, timeAdjustmentForm);

            _unitOfWork.TimeAdjustmentFormRepository.Update(timeAdjustmentForm);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.TimeAdjustmentFormTableName,
                userId,
                timeAdjustmentForm.Id);
        }
    }
}
