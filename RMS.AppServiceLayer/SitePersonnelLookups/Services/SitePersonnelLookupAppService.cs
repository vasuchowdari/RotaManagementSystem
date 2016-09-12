using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.SitePersonnelLookups.Dto;
using RMS.AppServiceLayer.SitePersonnelLookups.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;
using Site = System.Security.Policy.Site;

namespace RMS.AppServiceLayer.SitePersonnelLookups.Services
{
    public class SitePersonnelLookupAppService : ISitePersonnelLookupAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public SitePersonnelLookupAppService(IAuditLogAppService auditLogAppService)
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
        public SitePersonnelLookupDto GetById(long id)
        {
            var sitePersonnelLookup = _unitOfWork.SitePersonnelLookupRepository.GetById(id);

            if (sitePersonnelLookup != null)
            {
                var sitePersonnelLookupDto = Mapper.Map<SitePersonnelLookupDto>(sitePersonnelLookup);

                return sitePersonnelLookupDto;
            }

            return null;
        }

        public ICollection<SitePersonnelLookupDto> GetAll()
        {
            var sitePersonnelLookups = _unitOfWork.SitePersonnelLookupRepository.GetAll();
            var sitePersonnelLookupDtos = new List<SitePersonnelLookupDto>();

            if (sitePersonnelLookups != null)
            {
                foreach (var sitePersonnelLookup in sitePersonnelLookups)
                {
                    sitePersonnelLookupDtos.Add(Mapper.Map<SitePersonnelLookupDto>(sitePersonnelLookup));
                }

                return sitePersonnelLookupDtos;
            }

            return null;
        }

        public ICollection<SitePersonnelLookupDto> GetForIds(long? employeeId)
        {
            var sitePersonnelLookups = new List<SitePersonnelLookup>();
            var sitePersonnelLookupDtos = new List<SitePersonnelLookupDto>();

            if (employeeId != null)
            {
                sitePersonnelLookups = _unitOfWork.SitePersonnelLookupRepository.Get(spl => spl.EmployeeId == employeeId && spl.IsActive).ToList();

                foreach (var lookup in sitePersonnelLookups)
                {
                    sitePersonnelLookupDtos.Add(Mapper.Map<SitePersonnelLookupDto>(lookup));
                }

                return sitePersonnelLookupDtos;
            }

            return null;
        }

        public ICollection<SitePersonnelLookupDto> GetAllForEmployee(long employeeId)
        {
            // MANAGER CODE USING THIS FOR ROTA, MOST LIKELY
            var sitePersonnelLookups = _unitOfWork.SitePersonnelLookupRepository
                                                  .Get(spl => spl.EmployeeId == employeeId &&
                                                              spl.IsActive)
                                                  .ToList();

            var sitePersonnelLookupDtos = new List<SitePersonnelLookupDto>();

            if (sitePersonnelLookups != null)
            {
                foreach (var sitePersonnelLookup in sitePersonnelLookups)
                {
                    sitePersonnelLookupDtos.Add(Mapper.Map<SitePersonnelLookupDto>(sitePersonnelLookup));
                }

                return sitePersonnelLookupDtos;
            }

            return null;
        }

        public ICollection<SitePersonnelLookupDto> GetAllPlusInactiveForEmployee(long employeeId)
        {
            // ADMIN CODE FOR UPDATING RECORDS COULD DO WITH GETTING ALL, ACTIVE OR NOT
            var sitePersonnelLookups = _unitOfWork.SitePersonnelLookupRepository
                                                  .Get(spl => spl.EmployeeId == employeeId)
                                                  .ToList();

            var sitePersonnelLookupDtos = new List<SitePersonnelLookupDto>();

            if (sitePersonnelLookups != null)
            {
                foreach (var sitePersonnelLookup in sitePersonnelLookups)
                {
                    sitePersonnelLookupDtos.Add(Mapper.Map<SitePersonnelLookupDto>(sitePersonnelLookup));
                }

                return sitePersonnelLookupDtos;
            }

            return null;
        }

        public ICollection<SitePersonnelLookupDto> GetAllForSite(long siteId)
        {
            var sitePersonnelLookups = _unitOfWork.SitePersonnelLookupRepository.Get(spl => spl.SiteId == siteId);
            var sitePersonnelLookupDtos = new List<SitePersonnelLookupDto>();

            if (sitePersonnelLookups != null)
            {
                foreach (var sitePersonnelLookup in sitePersonnelLookups)
                {
                    sitePersonnelLookupDtos.Add(Mapper.Map<SitePersonnelLookupDto>(sitePersonnelLookup));
                }

                return sitePersonnelLookupDtos;
            }

            return null;
        }


        // CRUD
        public void Create(SitePersonnelLookupDto sitePersonnelLookupDto, long userId)
        {
            var sitePersonnelLookup = Mapper.Map<SitePersonnelLookup>(sitePersonnelLookupDto);

            _unitOfWork.SitePersonnelLookupRepository.Create(sitePersonnelLookup);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.SitePersonnelLookupTableName,
                userId,
                sitePersonnelLookup.Id);
        }

        public void Update(SitePersonnelLookupDto sitePersonnelLookupDto, long userId)
        {
            var sitePersonnelLookup = _unitOfWork.SitePersonnelLookupRepository.GetById(sitePersonnelLookupDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(sitePersonnelLookupDto, sitePersonnelLookup);

            _unitOfWork.SitePersonnelLookupRepository.Update(sitePersonnelLookup);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.SitePersonnelLookupTableName,
                userId,
                sitePersonnelLookup.Id);
        }

        public void Delete(long id, long userId)
        {
            var sitePersonnelLookup = _unitOfWork.SitePersonnelLookupRepository.GetById(id);

            sitePersonnelLookup.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.SitePersonnelLookupRepository.Update(sitePersonnelLookup);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.SitePersonnelLookupTableName,
                userId,
                sitePersonnelLookup.Id);
        }
    }
}
