using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Genders.Dto;
using RMS.AppServiceLayer.Genders.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Genders.Services
{
    public class GenderAppService : IGenderAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public GenderAppService(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        // Service Mehtods
        public void Dispose()
        {
            _unitOfWork.Dispose();
            _auditLogAppService.Dispose();
        }


        // Repo Methods
        public GenderDto GetById(long id)
        {
            var gender = _unitOfWork.GenderRepository.GetById(id);

            if (gender != null)
            {
                var genderDto = Mapper.Map<GenderDto>(gender);

                return genderDto;    
            }

            return null;
        }

        public ICollection<GenderDto> GetAll()
        {
            var genders = _unitOfWork.GenderRepository.GetAll();

            if (genders != null)
            {
                var genderDtos = new List<GenderDto>();

                foreach (var gender in genders)
                {
                    genderDtos.Add(Mapper.Map<GenderDto>(gender));
                }

                return genderDtos;    
            }

            return null;
        }


        // CRUD
        public void Create(GenderDto genderDto, long userId)
        {
            var gender = Mapper.Map<Gender>(genderDto);

            _unitOfWork.GenderRepository.Create(gender);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.GenderTableName,
                userId,
                gender.Id);
        }

        public void Update(GenderDto genderDto, long userId)
        {
            var gender = _unitOfWork.GenderRepository.GetById(genderDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(genderDto, gender);

            _unitOfWork.GenderRepository.Update(gender);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.GenderTableName,
                userId,
                gender.Id);
        }

        public void Delete(long id, long userId)
        {
            var gender = _unitOfWork.GenderRepository.GetById(id);

            gender.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.GenderRepository.Update(gender);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.GenderTableName,
                userId,
                gender.Id);
        }
    }
}
