using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.SubSiteTypes.Dto;
using RMS.AppServiceLayer.SubSiteTypes.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.SubSiteTypes.Services
{
    public class SubSiteTypeAppService : ISubSiteTypeAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public SubSiteTypeAppService(IAuditLogAppService auditLogAppService)
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
        public SubSiteTypeDto GetById(long id)
        {
            var subSiteType = _unitOfWork.SubSiteTypeRepository.GetById(id);

            if (subSiteType != null)
            {
                var subSiteTypeDto = Mapper.Map<SubSiteTypeDto>(subSiteType);

                return subSiteTypeDto;    
            }

            return null;
        }

        public ICollection<SubSiteTypeDto> GetAll()
        {
            var subSiteTypes = _unitOfWork.SubSiteTypeRepository.GetAll();
            var subSiteTypeDtos = new List<SubSiteTypeDto>();

            if (subSiteTypes != null)
            {
                foreach (var subSiteType in subSiteTypes)
                {
                    subSiteTypeDtos.Add(Mapper.Map<SubSiteTypeDto>(subSiteType));
                }

                return subSiteTypeDtos;    
            }

            return null;
        }


        // CRUD
        public void Create(SubSiteTypeDto subSiteTypeDto, long userId)
        {
            var subSiteType = Mapper.Map<SubSiteType>(subSiteTypeDto);

            _unitOfWork.SubSiteTypeRepository.Create(subSiteType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.SubSiteTypeTableName,
                userId,
                subSiteType.Id);
        }

        public void Update(SubSiteTypeDto subSiteTypeDto, long userId)
        {
            var subSiteType = _unitOfWork.SubSiteTypeRepository.GetById(subSiteTypeDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(subSiteTypeDto, subSiteType);

            _unitOfWork.SubSiteTypeRepository.Update(subSiteType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.SubSiteTypeTableName,
                userId,
                subSiteType.Id);
        }

        public void Delete(long id, long userId)
        {
            var subSiteType = _unitOfWork.SubSiteTypeRepository.GetById(id);

            subSiteType.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.SubSiteTypeRepository.Update(subSiteType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.SubSiteTypeTableName,
                userId,
                subSiteType.Id);
        }


        // Private Methods
        //private static void MapDtoToEntityForUpdating(SubSiteTypeDto subSiteTypeDto, SubSiteType subSiteType)
        //{
        //    subSiteType.Name = subSiteTypeDto.Name;
        //    subSiteType.IsActive = subSiteTypeDto.IsActive;
        //}
    }
}
