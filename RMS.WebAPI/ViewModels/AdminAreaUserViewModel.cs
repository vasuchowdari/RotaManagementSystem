using System.Collections.Generic;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.ViewModels
{
    public class AdminAreaUserViewModel
    {
        public UserModel UserModel { get; set; }
        public EmployeeModel EmployeeModel { get; set; }
        public ContractModel ContractModel { get; set; }
        public ICollection<SitePersonnelLookupModel> SiteAccessModels { get; set; }
    }
}