using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.ResourceRateModifiers.Dto;
using RMS.AppServiceLayer.ResourceRateModifiers.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.ResourceRateModifiers.Services
{
    public class ResourceRateModifierAppService : IResourceRateModifierAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ResourceRateModifierAppService(IAuditLogAppService auditLogAppService)
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
        public ResourceRateModifierDto GetById(long id)
        {
            var rateModifier = _unitOfWork.ResourceRateModifierRepository.GetById(id);

            if (rateModifier != null)
            {
                var rateModifierDto = Mapper.Map<ResourceRateModifierDto>(rateModifier);

                return rateModifierDto;
            }

            return null;
        }

        public ICollection<ResourceRateModifierDto> GetAll()
        {
            var rateModifiers = _unitOfWork.ResourceRateModifierRepository.GetAll();

            if (rateModifiers != null)
            {
                var rateModifierDtos = new List<ResourceRateModifierDto>();

                foreach (var rateModifier in rateModifiers)
                {
                    rateModifierDtos.Add(Mapper.Map<ResourceRateModifierDto>(rateModifier));
                }

                return rateModifierDtos;
            }

            return null;
        }


        // CRUD
        public void Create(ResourceRateModifierDto rateModifierDto, long userId)
        {
            var rateModifier = Mapper.Map<ResourceRateModifier>(rateModifierDto);

            _unitOfWork.ResourceRateModifierRepository.Create(rateModifier);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.ResourceRateModifierTableName,
                userId,
                rateModifier.Id);
        }

        public void Update(ResourceRateModifierDto rateModifierDto, long userId)
        {
            var rateModifier = _unitOfWork.ResourceRateModifierRepository.GetById(rateModifierDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(rateModifierDto, rateModifier);

            _unitOfWork.ResourceRateModifierRepository.Update(rateModifier);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.ResourceRateModifierTableName,
                userId,
                rateModifier.Id);
        }

        public void Delete(long id, long userId)
        {
            var rateModifier = _unitOfWork.ResourceRateModifierRepository.GetById(id);

            rateModifier.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.ResourceRateModifierRepository.Update(rateModifier);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.ResourceRateModifierTableName,
                userId,
                rateModifier.Id);
        }
    }
}
