using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.ShiftTypes.Dto;
using RMS.AppServiceLayer.ShiftTypes.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.ShiftTypes.Services
{
    public class ShiftTypeAppService : IShiftTypeAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public ShiftTypeAppService(IAuditLogAppService auditLogAppService)
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
        public ShiftTypeDto GetById(long id)
        {
            var shiftType = _unitOfWork.ShiftTypeRepository.GetById(id);

            if (shiftType != null)
            {
                var shiftTypeDto = Mapper.Map<ShiftTypeDto>(shiftType);

                return shiftTypeDto;
            }

            return null;
        }

        public ICollection<ShiftTypeDto> GetAll()
        {
            var shiftTypes = _unitOfWork.ShiftTypeRepository.GetAll();

            if (shiftTypes != null)
            {
                var shiftTypeDtos = new List<ShiftTypeDto>();

                foreach (var shiftType in shiftTypes)
                {
                    shiftTypeDtos.Add(Mapper.Map<ShiftTypeDto>(shiftType));
                }

                return shiftTypeDtos;
            }

            return null;
        }


        // CRUD
        public void Create(ShiftTypeDto shiftTypeDto, long userId)
        {
            var shiftType = Mapper.Map<ShiftType>(shiftTypeDto);

            _unitOfWork.ShiftTypeRepository.Create(shiftType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.ShiftTypeTableName,
                userId,
                shiftType.Id);
        }

        public void Update(ShiftTypeDto shiftTypeDto, long userId)
        {
            var shiftType = _unitOfWork.ShiftTypeRepository.GetById(shiftTypeDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(shiftTypeDto, shiftType);

            _unitOfWork.ShiftTypeRepository.Update(shiftType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.ShiftTypeTableName,
                userId,
                shiftType.Id);
        }

        public void Delete(long id, long userId)
        {
            var shiftType = _unitOfWork.ShiftTypeRepository.GetById(id);

            shiftType.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.ShiftTypeRepository.Update(shiftType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.ShiftTypeTableName,
                userId,
                shiftType.Id);
        }
    }
}
