using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.BudgetPeriods.Dto;

namespace RMS.AppServiceLayer.BudgetPeriods.Interfaces
{
    public interface IBudgetPeriodAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        BudgetPeriodDto GetById(long id);
        ICollection<BudgetPeriodDto> GetAll();

        // CRUD
        void Create(BudgetPeriodDto budgetPeriodDto, long userId);
        void Update(BudgetPeriodDto budgetPeriodDto, long userId);
        void Delete(long id, long userId);
    }
}
