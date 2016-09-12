using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Budgets.Dto;

namespace RMS.AppServiceLayer.Budgets.Interfaces
{
    public interface IBudgetAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        BudgetDto GetById(long id);
        ICollection<BudgetDto> GetAll();

        // CRUD
        void Create(BudgetDto budgetDto, long userId);
        void Update(BudgetDto budgetDto, long userId);
        void Delete(long id, long userId);
    }
}
