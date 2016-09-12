using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.SiteTypes.Dto;
using RMS.AppServiceLayer.SiteTypes.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.SiteTypes.Services
{
    public class SiteTypeAppService : ISiteTypeAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public SiteTypeAppService(IAuditLogAppService auditLogAppService)
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
        public SiteTypeDto GetById(long id)
        {
            var siteType = _unitOfWork.SiteTypeRepository.GetById(id);

            if (siteType != null)
            {
                var siteTypeDto = Mapper.Map<SiteTypeDto>(siteType);

                return siteTypeDto;    
            }

            return null;
        }

        public ICollection<SiteTypeDto> GetAll()
        {
            var siteTypes = _unitOfWork.SiteTypeRepository.GetAll();
            var siteTypeDtos = new List<SiteTypeDto>();

            if (siteTypes != null)
            {
                foreach (var siteType in siteTypes)
                {
                    siteTypeDtos.Add(Mapper.Map<SiteTypeDto>(siteType));
                }

                return siteTypeDtos;    
            }

            return null;
        }


        // CRUD
        public void Create(SiteTypeDto siteTypeDto, long userId)
        {
            var siteType = Mapper.Map<SiteType>(siteTypeDto);

            _unitOfWork.SiteTypeRepository.Create(siteType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.SiteTypeTableName,
                userId,
                siteType.Id);
        }

        public void Update(SiteTypeDto siteTypeDto, long userId)
        {
            var siteType = _unitOfWork.SiteTypeRepository.GetById(siteTypeDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(siteTypeDto, siteType);

            _unitOfWork.SiteTypeRepository.Update(siteType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.SiteTypeTableName,
                userId,
                siteType.Id);
        }

        public void Delete(long id, long userId)
        {
            var siteType = _unitOfWork.SiteTypeRepository.GetById(id);

            siteType.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.SiteTypeRepository.Update(siteType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.SiteTypeTableName,
                userId,
                siteType.Id);
        }


        // Private Methods
        //private static void MapDtoToEntityForUpdating(SiteTypeDto siteTypeDto, SiteType siteType)
        //{
        //    siteType.Name = siteTypeDto.Name;
        //    siteType.IsActive = siteTypeDto.IsActive;
        //}
    }
}
