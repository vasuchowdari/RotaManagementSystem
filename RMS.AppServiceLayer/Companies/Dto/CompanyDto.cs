using System.Collections.Generic;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.Employees.Dto;

namespace RMS.AppServiceLayer.Companies.Dto
{
    public class CompanyDto : BaseDto
    {
    //    public long Id { get; set; }
    //    public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }

        public ICollection<EmployeeDto> EmployeeDtos { get; set; } 
    }
}
