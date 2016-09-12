using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.SubSites.Dto;
using RMS.AppServiceLayer.SubSites.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.SubSites.Services
{
    public class SubSiteAppService : ISubSiteAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public SubSiteAppService(IAuditLogAppService auditLogAppService)
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
        public SubSiteDto GetById(long id)
        {
            var subSite = _unitOfWork.SubSiteRepository.GetById(id);

            if (subSite != null)
            {
                var subSiteDto = Mapper.Map<SubSiteDto>(subSite);

                return subSiteDto;    
            }

            return null;
        }

        public ICollection<SubSiteDto> GetAll()
        {
            var subSites = _unitOfWork.SubSiteRepository.GetAll();
            var subSiteDtos = new List<SubSiteDto>();

            if (subSites != null)
            {
                foreach (var subSite in subSites)
                {
                    subSiteDtos.Add(Mapper.Map<SubSiteDto>(subSite));
                }

                return subSiteDtos;    
            }

            return null;
        }


        // CRUD
        public void Create(SubSiteDto subSiteDto, long userId)
        {
            var subSite = Mapper.Map<SubSite>(subSiteDto);

            _unitOfWork.SubSiteRepository.Create(subSite);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.SubSiteTableName,
                userId,
                subSite.Id);
        }

        public void Update(SubSiteDto subSiteDto, long userId)
        {
            var subSite = _unitOfWork.SubSiteRepository.GetById(subSiteDto.Id);
            
            CommonHelperAppService.MapDtoToEntityForUpdating(subSiteDto, subSite);

            _unitOfWork.SubSiteRepository.Update(subSite);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.SubSiteTableName,
                userId,
                subSite.Id);
        }

        public void Delete(long id, long userId)
        {
            var subSite = _unitOfWork.SubSiteRepository.GetById(id);

            subSite.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.SubSiteRepository.Update(subSite);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.SubSiteTableName,
                userId,
                subSite.Id);
        }


        // Private Methods
        //private static void MapDtoToEntityForUpdating(SubSiteDto subSiteDto, SubSite subSite)
        //{
        //    subSite.IsActive = subSiteDto.IsActive;
        //    subSite.Name = subSiteDto.Name;
        //    subSite.SiteId = subSiteDto.SiteId;
        //    subSite.SubSiteTypeId = subSiteDto.SubSiteTypeId;
        //}
    }
}
