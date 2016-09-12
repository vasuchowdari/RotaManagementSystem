using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Companies.Dto;

namespace RMS.AppServiceLayer.Companies.Interfaces
{
    public interface ICompanyAppService : IDisposable
    {
        // Service Methods


        // Repo Methods
        CompanyDto GetById(long id);
        CompanyDto GetByIdWithEmployees(long id);
        ICollection<CompanyDto> GetAll(); 


        // CRUD
        void Create(CompanyDto compannyDto, long userId);
        void Update(CompanyDto companyDto, long userId);
        void Delete(long id, long userId);
    }
}
