using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Contracts.Dto;
using RMS.AppServiceLayer.Contracts.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Contracts.Services
{
    public class ContractAppService : IContractAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public ContractAppService(IAuditLogAppService auditLogAppService)
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
        public ContractDto GetById(long id)
        {
            var contract = _unitOfWork.ContractRepository.GetById(id);

            if (contract != null)
            {
                var contractDto = Mapper.Map<ContractDto>(contract);

                return contractDto;
            }

            return null;
        }

        public ICollection<ContractDto> GetAll()
        {
            var contracts = _unitOfWork.ContractRepository.GetAll();

            if (contracts != null)
            {
                var contractDtos = new List<ContractDto>();

                foreach (var contract in contracts)
                {
                    contractDtos.Add(Mapper.Map<ContractDto>(contract));
                }

                return contractDtos;
            }

            return null;
        }

        public ContractDto GetContractForEmployee(long employeeId)
        {
            var contract = _unitOfWork.ContractRepository.Get(c => c.EmployeeId == employeeId && c.IsActive);

            if (contract != null)
            {
                var contractDto = Mapper.Map<ContractDto>(contract);

                return contractDto;
            }

            return null;
        }


        // CRUD
        public void Create(ContractDto contractDto, long userId)
        {
            var contract = Mapper.Map<Contract>(contractDto);

            _unitOfWork.ContractRepository.Create(contract);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.ContractTableName,
                userId,
                contract.Id);
        }

        public void CreateRange(ICollection<ContractDto> contractDtos, long userId)
        {
            var contracts = new List<Contract>();

            foreach (var contractDto in contractDtos)
            {
                contracts.Add(Mapper.Map<Contract>(contractDto));
            }

            _unitOfWork.ContractRepository.CreateRange(contracts);
            _unitOfWork.Save();


            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.ContractTableName,
                userId,
                1001);
        }

        public void Update(ContractDto contractDto, long userId)
        {
            var contract = _unitOfWork.ContractRepository.GetById(contractDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(contractDto, contract);

            _unitOfWork.ContractRepository.Update(contract);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.ContractTableName,
                userId,
                contract.Id);
        }

        public void Delete(long id, long userId)
        {
            var contract = _unitOfWork.ContractRepository.GetById(id);

            contract.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.ContractRepository.Update(contract);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.ContractTableName,
                userId,
                contract.Id);
        }
    }
}
