using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Budgets.Dto;
using RMS.AppServiceLayer.Budgets.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Budgets.Services
{

    public class BudgetAppService : IBudgetAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public BudgetAppService(IAuditLogAppService auditLogAppService)
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
        public BudgetDto GetById(long id)
        {
            var budget = _unitOfWork.BudgetRepository.GetById(id);

            if (budget != null)
            {
                var budgetDto = Mapper.Map<BudgetDto>(budget);

                return budgetDto;
            }

            return null;
        }

        public ICollection<BudgetDto> GetAll()
        {
            var budgets = _unitOfWork.BudgetRepository.GetAll();

            if (budgets != null)
            {
                var budgetDtos = new List<BudgetDto>();

                foreach (var budget in budgets)
                {
                    budgetDtos.Add(Mapper.Map<BudgetDto>(budget));
                }

                return budgetDtos;
            }

            return null;
        }


        // CRUD
        public void Create(BudgetDto budgetDto, long userId)
        {
            var budget = Mapper.Map<Budget>(budgetDto);

            _unitOfWork.BudgetRepository.Create(budget);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.BudgetTableName,
                userId,
                budget.Id);
        }

        public void Update(BudgetDto budgetDto, long userId)
        {
            var budget = _unitOfWork.BudgetRepository.GetById(budgetDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(budgetDto, budget);

            _unitOfWork.BudgetRepository.Update(budget);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.BudgetTableName,
                userId,
                budget.Id);
        }

        public void Delete(long id, long userId)
        {
            var budget = _unitOfWork.BudgetRepository.GetById(id);

            budget.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.BudgetRepository.Update(budget);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.BudgetTableName,
                userId,
                budget.Id);
        }
    }
}
