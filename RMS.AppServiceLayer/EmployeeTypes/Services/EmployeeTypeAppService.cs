using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.EmployeeTypes.Dto;
using RMS.AppServiceLayer.EmployeeTypes.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.EmployeeTypes.Services
{
    public class EmployeeTypeAppService : IEmployeeTypeAppService
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IAuditLogAppService _auditLogAppService;

        public EmployeeTypeAppService(IAuditLogAppService auditLogAppService)
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
        public EmployeeTypeDto GetById(long id)
        {
            var employeeType = _unitOfWork.EmployeeTypeRepository.GetById(id);

            if (employeeType != null)
            {
                var employeeTypeDto = Mapper.Map<EmployeeTypeDto>(employeeType);

                return employeeTypeDto;    
            }

            return null;
        }

        public ICollection<EmployeeTypeDto> GetAll()
        {
            var employeeTypes = _unitOfWork.EmployeeTypeRepository.GetAll();
            var employeeTypeDtos = new List<EmployeeTypeDto>();

            if (employeeTypes != null)
            {
                foreach (var employeeType in employeeTypes)
                {
                    employeeTypeDtos.Add(Mapper.Map<EmployeeTypeDto>(employeeType));
                }

                return employeeTypeDtos;    
            }

            return null;
        }


        // CRUD
        public void Create(EmployeeTypeDto employeeTypeDto, long userId)
        {
            var employeeType = Mapper.Map<EmployeeType>(employeeTypeDto);

            _unitOfWork.EmployeeTypeRepository.Create(employeeType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.EmployeeTypeTableName,
                userId,
                employeeType.Id);
        }

        public void Update(EmployeeTypeDto employeeTypeDto, long userId)
        {
            var employeeType = _unitOfWork.EmployeeTypeRepository.GetById(employeeTypeDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(employeeTypeDto, employeeType);

            _unitOfWork.EmployeeTypeRepository.Update(employeeType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.EmployeeTypeTableName,
                userId,
                employeeType.Id);
        }

        public void Delete(long id, long userId)
        {
            var employeeType = _unitOfWork.EmployeeTypeRepository.GetById(id);

            employeeType.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.EmployeeTypeRepository.Update(employeeType);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.EmployeeTypeTableName,
                userId,
                employeeType.Id);
        }


        // Private Methods
        //private static void MapDtoToEntityForUpdating(EmployeeTypeDto employeeTypeDto, EmployeeType employeeType)
        //{
        //    employeeType.Name = employeeTypeDto.Name;
        //    employeeType.IsActive = employeeTypeDto.IsActive;
        //}
    }
}
