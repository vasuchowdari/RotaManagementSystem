using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using RMS.AppServiceLayer.AuditLogs.Interfaces;
using RMS.AppServiceLayer.Companies.Dto;
using RMS.AppServiceLayer.Helpers;
using RMS.AppServiceLayer.Companies.Interfaces;
using RMS.Core;
using RMS.Infrastructure.EF;

namespace RMS.AppServiceLayer.Companies.Services
{
    public class CompanyAppService : ICompanyAppService
    {
        private readonly IAuditLogAppService _auditLogAppService;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public CompanyAppService(IAuditLogAppService auditLogAppService)
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
        public CompanyDto GetById(long id)
        {
            var company = _unitOfWork.CompanyRepository.GetById(id);

            if (company != null)
            {
                var companyDto = Mapper.Map<CompanyDto>(company);

                return companyDto;    
            }

            return null;
        }

        public CompanyDto GetByIdWithEmployees(long id)
        {
            var company = _unitOfWork.CompanyRepository.Get(c => c.Id == id, null, "Employees").FirstOrDefault();

            if (company != null)
            {
                var companyDto = Mapper.Map<CompanyDto>(company);

                return companyDto;
            }

            return null;
        }

        public ICollection<CompanyDto> GetAll()
        {
            var companies = _unitOfWork.CompanyRepository.GetAll();

            if (companies != null)
            {
                var companyDtos = new List<CompanyDto>();

                foreach (var company in companies)
                {
                    companyDtos.Add(Mapper.Map<CompanyDto>(company));
                }

                return companyDtos;
            }

            return null;
        }


        // CRUD
        public void Create(CompanyDto compannyDto, long userId)
        {
            var company = Mapper.Map<Company>(compannyDto);

            _unitOfWork.CompanyRepository.Create(company);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeCreate,
                AppConstants.CompanyTableName,
                userId,
                company.Id);
        }

        public void Update(CompanyDto companyDto, long userId)
        {
            var company = _unitOfWork.CompanyRepository.GetById(companyDto.Id);

            CommonHelperAppService.MapDtoToEntityForUpdating(companyDto, company);

            _unitOfWork.CompanyRepository.Update(company);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeUpdate,
                AppConstants.CompanyTableName,
                userId,
                company.Id);
        }

        public void Delete(long id, long userId)
        {
            var company = _unitOfWork.CompanyRepository.GetById(id);

            company.IsActive = CommonHelperAppService.DeleteEntity();

            _unitOfWork.CompanyRepository.Update(company);
            _unitOfWork.Save();

            // Audit
            _auditLogAppService.Audit(
                AppConstants.ActionTypeDelete,
                AppConstants.CompanyTableName,
                userId,
                company.Id);
        }
    }
}
