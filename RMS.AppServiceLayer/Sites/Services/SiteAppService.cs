using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Sites.Dto;
using RMS.AppServiceLayer.Sites.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Sites.Services
{
    public class SiteAppService : ISiteAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public SiteAppService(IAuditLogAppService auditLogAppService)
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
        public SiteDto GetById(long id)
        {
            var site = _unitOfWork.SiteRepository.GetById(id);

            if (site != null)
            {
                var siteDto = Mapper.Map<SiteDto>(site);

                return siteDto;
            }

            return null;
        }

        public ICollection<SiteDto> GetAll()
        {
            //var sites = _unitOfWork.SiteRepository.GetAll();
            var sites = _unitOfWork.SiteRepository.Get(s => s.IsActive, null, "SubSites").ToList();

            if (sites != null)
            {
                var siteDtos = new List<SiteDto>();

                foreach (var site in sites)
                {
                    siteDtos.Add(Mapper.Map<SiteDto>(site));
                }

                return siteDtos;
            }

            return null;
        }

        public ICollection<SiteDto> GetForCompany(long companyId)
        {
            var sites = _unitOfWork.SiteRepository
                                   .Get(s => s.CompanyId == companyId && s.IsActive,
                                        null,
                                        "SubSites")
                                   .ToList();

            if (sites != null)
            {
                var siteDtos = new List<SiteDto>();

                foreach (var site in sites)
                {
                    siteDtos.Add(Mapper.Map<SiteDto>(site));
                }

                return siteDtos;
            }

            return null;
        }


        // CRUD
        public void Create(SiteDto siteDto, long userId)
        {
            var site = Mapper.Map<Site>(siteDto);

            _unitOfWork.SiteRepository.Create(site);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.SiteTableName,
                userId,
                site.Id);
        }

        public void Update(SiteDto siteDto, long userId)
        {
            var site = _unitOfWork.SiteRepository.GetById(siteDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(siteDto, site);

            _unitOfWork.SiteRepository.Update(site);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.SiteTableName,
                userId,
                site.Id);
        }

        public void Delete(long id, long userId)
        {
            var site = _unitOfWork.SiteRepository.GetById(id);

            site.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.SiteRepository.Update(site);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.SiteTableName,
                userId,
                site.Id);
        }
    }
}