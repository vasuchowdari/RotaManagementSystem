using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.SystemAccessRoles.Dto;
using RMS.AppServiceLayer.SystemAccessRoles.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.SystemAccessRoles.Services
{
    public class SystemAccessRoleAppService : ISystemAccessRoleAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public SystemAccessRoleAppService(IAuditLogAppService auditLogAppService)
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
        public SystemAccessRoleDto GetById(long id)
        {
            var systemAccessRole = _unitOfWork.SystemAccessRoleRepository.GetById(id);

            if (systemAccessRole != null)
            {
                var systemAccessRoleDto = Mapper.Map<SystemAccessRoleDto>(systemAccessRole);

                return systemAccessRoleDto;    
            }

            return null;
        }

        public ICollection<SystemAccessRoleDto> GetAll()
        {
            var systemAccessRoles = _unitOfWork.SystemAccessRoleRepository.GetAll();
            var systemAccessRoleDtos = new List<SystemAccessRoleDto>();

            if (systemAccessRoles != null)
            {
                foreach (var systemAccessRole in systemAccessRoles)
                {
                    systemAccessRoleDtos.Add(Mapper.Map<SystemAccessRoleDto>(systemAccessRole));
                }

                return systemAccessRoleDtos;    
            }

            return null;
        }


        // CRUD
        public void Create(SystemAccessRoleDto systemAccessRoleDto, long userId)
        {
            var systemAccessRole = Mapper.Map<SystemAccessRole>(systemAccessRoleDto);

            _unitOfWork.SystemAccessRoleRepository.Create(systemAccessRole);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.SystemAccessRoleTableName,
                userId,
                systemAccessRole.Id);
        }

        public void Update(SystemAccessRoleDto systemAccessRoleDto, long userId)
        {
            var systemAccessRole = _unitOfWork.SystemAccessRoleRepository.GetById(systemAccessRoleDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(systemAccessRoleDto, systemAccessRole);

            _unitOfWork.SystemAccessRoleRepository.Update(systemAccessRole);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.SystemAccessRoleTableName,
                userId,
                systemAccessRole.Id);
        }

        public void Delete(long id, long userId)
        {
            var systemAccessRole = _unitOfWork.SystemAccessRoleRepository.GetById(id);

            // trip flag to 'Delete'
            systemAccessRole.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.SystemAccessRoleRepository.Update(systemAccessRole);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.SystemAccessRoleTableName,
                userId,
                systemAccessRole.Id);
        }
    }
}
