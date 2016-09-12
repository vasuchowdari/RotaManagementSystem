using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.Companies.Dto;
using RMS.AppServiceLayer.Contracts.Dto;
using RMS.AppServiceLayer.EmployeeTypes.Dto;
using RMS.AppServiceLayer.LeaveRequests.Dto;
using RMS.AppServiceLayer.PersonnelLeaveProfiles.Dto;
using RMS.AppServiceLayer.Users.Dto;

namespace RMS.AppServiceLayer.Employees.Dto
{
    public class EmployeeDto : BaseDto
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public long GenderId { get; set; }
        public long BaseSiteId { get; set; }
        public long? BaseSubSiteId { get; set; }

        public long CompanyId { get; set; }
        public long UserId { get; set; }
        public long EmployeeTypeId { get; set; }

        public CompanyDto CompanyDto { get; set; }
        public UserDto UserDto { get; set; }
        public EmployeeTypeDto EmployeeTypeDto { get; set; }
        public ICollection<ContractDto> ContractDtos { get; set; }

        public ICollection<LeaveRequestDto> LeaveRequestDtos { get; set; }
        public ICollection<PersonnelLeaveProfileDto> LeaveProfileDtos { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? LeaveDate { get; set; }
    }
}
