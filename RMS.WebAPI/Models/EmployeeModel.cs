using System;
using System.Collections.Generic;

namespace RMS.WebAPI.Models
{
    public class EmployeeModel : BaseModel
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

        public CompanyModel CompanyModel { get; set; }
        public UserModel UserModel { get; set; }
        public EmployeeTypeModel EmployeeTypeModel { get; set; }
        public ICollection<ContractModel> ContractModels { get; set; }

        public ICollection<LeaveRequestModel> LeaveRequestModels { get; set; }
        public ICollection<PersonnelLeaveProfileModel> LeaveProfileModels { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? LeaveDate { get; set; }
    }
}