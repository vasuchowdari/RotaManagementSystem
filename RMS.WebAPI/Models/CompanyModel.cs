using System.Collections.Generic;

namespace RMS.WebAPI.Models
{
    public class CompanyModel : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }

        public ICollection<EmployeeModel> EmployeeModels { get; set; }
    }
}