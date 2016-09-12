using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.LeaveTypes.Dto;
using RMS.AppServiceLayer.LeaveTypes.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.LeaveTypes.Services
{
    public class LeaveTypeAppService : ILeaveTypeAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public LeaveTypeAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }


        // Repo Methods
        public LeaveTypeDto GetById(long id)
        {
            var leaveType = _unitOfWork.LeaveTypeRepository.GetById(id);

            if (leaveType != null)
            {
                var leaveTypeDto = Mapper.Map<LeaveTypeDto>(leaveType);

                return leaveTypeDto;
            }

            return null;
        }

        public ICollection<LeaveTypeDto> GetAll()
        {
            var leaveTypes = _unitOfWork.LeaveTypeRepository.GetAll();

            if (leaveTypes != null)
            {
                var leaveTypeDtos = new List<LeaveTypeDto>();

                foreach (var leaveType in leaveTypes)
                {
                    leaveTypeDtos.Add(Mapper.Map<LeaveTypeDto>(leaveType));
                }

                return leaveTypeDtos;
            }

            return null;
        }


        // CRUD
        public void Create(LeaveTypeDto leaveTypeDto, long userId)
        {
            var leaveType = Mapper.Map<LeaveType>(leaveTypeDto);

            _unitOfWork.LeaveTypeRepository.Create(leaveType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.LeaveTypeTableName,
                userId,
                leaveType.Id);
        }

        public void Update(LeaveTypeDto leaveTypeDto, long userId)
        {
            var leaveType = _unitOfWork.LeaveTypeRepository.GetById(leaveTypeDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(leaveTypeDto, leaveType);

            _unitOfWork.LeaveTypeRepository.Update(leaveType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.LeaveTypeTableName,
                userId,
                leaveType.Id);
        }

        public void Delete(long id, long userId)
        {
            var leaveType = _unitOfWork.LeaveTypeRepository.GetById(id);

            leaveType.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.LeaveTypeRepository.Update(leaveType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.LeaveTypeTableName,
                userId,
                leaveType.Id);
        }
    }
}
