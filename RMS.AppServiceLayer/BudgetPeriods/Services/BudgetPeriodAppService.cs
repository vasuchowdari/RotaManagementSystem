using System.Collections.Generic;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.BudgetPeriods.Dto;
using RMS.AppServiceLayer.BudgetPeriods.Interfaces;
using RMS.AppServiceLayer.Helpers;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.BudgetPeriods.Services
{
    public class BudgetPeriodAppService : IBudgetPeriodAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public BudgetPeriodAppService(IAuditLogAppService auditLogAppService)
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
        public BudgetPeriodDto GetById(long id)
        {
            var budgetPeriod = _unitOfWork.BudgetPeriodRepository.GetById(id);

            if (budgetPeriod != null)
            {
                var budgetPeriodDto = Mapper.Map<BudgetPeriodDto>(budgetPeriod);

                return budgetPeriodDto;
            }

            return null;
        }

        public ICollection<BudgetPeriodDto> GetAll()
        {
            var budgetPeriods = _unitOfWork.BudgetPeriodRepository.GetAll();

            if (budgetPeriods != null)
            {
                var budgetPeriodDtos = new List<BudgetPeriodDto>();

                foreach (var budgetPeriod in budgetPeriods)
                {
                    budgetPeriodDtos.Add(Mapper.Map<BudgetPeriodDto>(budgetPeriod));
                }

                return budgetPeriodDtos;
            }

            return null;
        }


        // CRUD
        public void Create(BudgetPeriodDto budgetPeriodDto, long userId)
        {
            var budgetPeriod = Mapper.Map<BudgetPeriod>(budgetPeriodDto);

            _unitOfWork.BudgetPeriodRepository.Create(budgetPeriod);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.BudgetPeriodTableName,
                userId,
                budgetPeriod.Id);
        }

        public void Update(BudgetPeriodDto budgetPeriodDto, long userId)
        {
            var budgetPeriod = _unitOfWork.BudgetPeriodRepository.GetById(budgetPeriodDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(budgetPeriodDto, budgetPeriod);

            _unitOfWork.BudgetPeriodRepository.Update(budgetPeriod);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.BudgetPeriodTableName,
                userId,
                budgetPeriod.Id);
        }

        public void Delete(long id, long userId)
        {
            var budgetPeriod = _unitOfWork.BudgetPeriodRepository.GetById(id);

            budgetPeriod.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.BudgetPeriodRepository.Update(budgetPeriod);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.BudgetPeriodTableName,
                userId,
                budgetPeriod.Id);
        }
    }
}
