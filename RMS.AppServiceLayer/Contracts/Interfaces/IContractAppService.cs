using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Contracts.Dto;

namespace RMS.AppServiceLayer.Contracts.Interfaces
{
    public interface IContractAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        ContractDto GetById(long id);
        ICollection<ContractDto> GetAll();
        ContractDto GetContractForEmployee(long employeeId);

        // CRUD
        void Create(ContractDto contractDto, long userId);
        void CreateRange(ICollection<ContractDto> contractDtos, long userId);
        void Update(ContractDto contractDto, long userId);
        void Delete(long id, long userId);
    }
}
