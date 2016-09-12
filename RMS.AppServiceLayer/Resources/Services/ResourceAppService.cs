using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Resources.Dto;
using RMS.AppServiceLayer.Resources.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Resources.Services
{
    public class ResourceAppService : IResourceAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ResourceAppService(IAuditLogAppService auditLogAppService)
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
        public ResourceDto GetById(long id)
        {
            var resource = _unitOfWork.ResourceRepository.GetById(id);

            if (resource != null)
            {
                var resourceDto = Mapper.Map<ResourceDto>(resource);

                return resourceDto;
            }

            return null;
        }

        public ICollection<ResourceDto> GetAll()
        {
            var resources = _unitOfWork.ResourceRepository.GetAll();

            if (resources != null)
            {
                var resourceDtos = new List<ResourceDto>();

                foreach (var resource in resources)
                {
                    resourceDtos.Add(Mapper.Map<ResourceDto>(resource));
                }

                return resourceDtos;
            }

            return null;
        }


        // CRUD
        public void Create(ResourceDto resourceDto, long userId)
        {
            var resource = Mapper.Map<Resource>(resourceDto);

            _unitOfWork.ResourceRepository.Create(resource);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.ResourceTableName,
                userId,
                resource.Id);
        }

        public void Update(ResourceDto resourceDto, long userId)
        {
            var resource = _unitOfWork.ResourceRepository.GetById(resourceDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(resourceDto, resource);

            _unitOfWork.ResourceRepository.Update(resource);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.ResourceTableName,
                userId,
                resource.Id);
        }

        public void Delete(long id, long userId)
        {
            var resource = _unitOfWork.ResourceRepository.GetById(id);

            resource.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.ResourceRepository.Update(resource);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.ResourceTableName,
                userId,
                resource.Id);
        }
    }
}
