using System;
using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Dto;
using RMS.AppServiceLayer.UserDefinedSystemConfigs.Interfaces;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.UserDefinedSystemConfigs.Services
{
    public class UserDefinedSystemConfigAppService : IUserDefinedSystemConfigAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public UserDefinedSystemConfigAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }

        public ICollection<UserDefinedSystemConfigDto> GetAll()
        {
            var systemConfigs = _unitOfWork.UserDefinedSystemConfigRepository.GetAll();

            if (systemConfigs != null)
            {
                var systemConfigDtos = new List<UserDefinedSystemConfigDto>();

                foreach (var systemConfig in systemConfigs)
                {
                    systemConfigDtos.Add(Mapper.Map<UserDefinedSystemConfigDto>(systemConfig));
                }

                return systemConfigDtos;
            }

            return null;
        }

        public void Update(UserDefinedSystemConfigDto udscDto, long userId)
        {
            var systemConfig = _unitOfWork.UserDefinedSystemConfigRepository.GetById(udscDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(udscDto, systemConfig);

            _unitOfWork.UserDefinedSystemConfigRepository.Update(systemConfig);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.UserDefinedSystemConfigTableName,
                userId,
                systemConfig.Id);
        }
    }
}
