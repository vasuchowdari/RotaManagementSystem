using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Dto;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.PersonnelLeaveProfiles.Services
{
    public class PersonnelLeaveProfileAppService : IPersonnelLeaveProfileAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public PersonnelLeaveProfileAppService(IAuditLogAppService auditLogAppService)
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
        public PersonnelLeaveProfileDto GetById(long id)
        {
            var leaveProfile = _unitOfWork.PersonnelLeaveProfileRepository.GetById(id);

            if (leaveProfile != null)
            {
                var leaveProfileDto = Mapper.Map<PersonnelLeaveProfileDto>(leaveProfile);

                return leaveProfileDto;
            }

            return null;
        }

        public ICollection<PersonnelLeaveProfileDto> GetAll()
        {
            var leaveProfiles = _unitOfWork.PersonnelLeaveProfileRepository.GetAll();

            if (leaveProfiles != null)
            {
                var leaveProfileDtos = new List<PersonnelLeaveProfileDto>();

                foreach (var leaveProfile in leaveProfiles)
                {
                    leaveProfileDtos.Add(Mapper.Map<PersonnelLeaveProfileDto>(leaveProfile));
                }

                return leaveProfileDtos;
            }

            return null;
        }


        // CRUD
        public void Create(PersonnelLeaveProfileDto personnelLeaveProfileDto, long userId)
        {
            var leaveProfile = Mapper.Map<PersonnelLeaveProfile>(personnelLeaveProfileDto);

            _unitOfWork.PersonnelLeaveProfileRepository.Create(leaveProfile);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.PersonnelLeaveProfileTableName,
                userId,
                leaveProfile.Id);
        }

        public void Update(PersonnelLeaveProfileDto personnelLeaveProfileDto, long userId)
        {
            var leaveProfile = _unitOfWork.PersonnelLeaveProfileRepository.GetById(personnelLeaveProfileDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(personnelLeaveProfileDto, leaveProfile);

            _unitOfWork.PersonnelLeaveProfileRepository.Update(leaveProfile);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.PersonnelLeaveProfileTableName,
                userId,
                leaveProfile.Id);
        }

        public void Delete(long id, long userId)
        {
            var leaveProfile = _unitOfWork.PersonnelLeaveProfileRepository.GetById(id);

            leaveProfile.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.PersonnelLeaveProfileRepository.Update(leaveProfile);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.PersonnelLeaveProfileTableName,
                userId,
                leaveProfile.Id);
        }
    }
}
