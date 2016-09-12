using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.ShiftTemplates.Dto;
using RMS.AppServiceLayer.ShiftTemplates.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.ShiftTemplates.Services
{
    public class ShiftTemplateAppService : IShiftTemplateAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ShiftTemplateAppService(IAuditLogAppService audtLogAppService)
        {
            _auditLogAppService = audtLogAppService;
        }


        // Service Methods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }


        // Repo Methods
        public ShiftTemplateDto GetById(long id)
        {
            var shiftTemplate = _unitOfWork.ShiftTemplateRepository.GetById(id);

            if (shiftTemplate != null)
            {
                var shiftTemplateDto = Mapper.Map<ShiftTemplateDto>(shiftTemplate);

                return shiftTemplateDto;
            }

            return null;
        }

        public ICollection<ShiftTemplateDto> GetAll()
        {
            var shiftTemplates = _unitOfWork.ShiftTemplateRepository.GetAll();

            if (shiftTemplates != null)
            {
                var shiftTemplateDtos = new List<ShiftTemplateDto>();

                foreach (var shiftTemplate in shiftTemplates)
                {
                    shiftTemplateDtos.Add(Mapper.Map<ShiftTemplateDto>(shiftTemplate));
                }

                return shiftTemplateDtos;
            }

            return null;
        }

        public ICollection<ShiftTemplateDto> GetForSite(long siteId)
        {
            var shiftTemplates = _unitOfWork.ShiftTemplateRepository
                .Get(st => st.SiteId == siteId && st.IsActive);

            if (shiftTemplates != null)
            {
                var shiftTemplateDtos = new List<ShiftTemplateDto>();

                foreach (var shiftTemplate in shiftTemplates)
                {
                    shiftTemplateDtos.Add(Mapper.Map<ShiftTemplateDto>(shiftTemplate));
                }

                return shiftTemplateDtos;
            }

            return null;
        }

        public ICollection<ShiftTemplateDto> GetBySearchCriteria(long resourceId, long siteId, long? subsiteId)
        {
            var shiftTemplates = _unitOfWork.ShiftTemplateRepository.Get(st => st.ResourceId == resourceId &&
                                                                               st.SiteId == siteId &&
                                                                               st.SubSiteId == subsiteId);
            if (shiftTemplates != null)
            {
                var shiftTemplateDtos = new List<ShiftTemplateDto>();

                foreach (var shiftTemplate in shiftTemplates)
                {
                    shiftTemplateDtos.Add(Mapper.Map<ShiftTemplateDto>(shiftTemplate));
                }

                return shiftTemplateDtos;
            }

            return null;
        }


        // CRUD
        public void Create(ShiftTemplateDto shiftTemplateDto, long userId)
        {
            var shiftTemplate = Mapper.Map<ShiftTemplate>(shiftTemplateDto);

            _unitOfWork.ShiftTemplateRepository.Create(shiftTemplate);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.ShiftTemplateTableName,
                userId,
                shiftTemplate.Id);
        }

        public void Update(ShiftTemplateDto shiftTemplateDto, long userId)
        {
            var shiftTemplate = _unitOfWork.ShiftTemplateRepository.GetById(shiftTemplateDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(shiftTemplateDto, shiftTemplate);

            _unitOfWork.ShiftTemplateRepository.Update(shiftTemplate);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.ShiftTemplateTableName,
                userId,
                shiftTemplate.Id);
        }

        public void Delete(long id, long userId)
        {
            var shiftTemplate = _unitOfWork.ShiftTemplateRepository.GetById(id);

            shiftTemplate.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.ShiftTemplateRepository.Update(shiftTemplate);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.ShiftTemplateTableName,
                userId,
                shiftTemplate.Id);
        }
    }
}
