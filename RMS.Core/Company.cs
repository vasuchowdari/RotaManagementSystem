using System.Collections.Generic;

namespace RMS.Core
{
    public class Company : BaseOrganisation
    {
        public string ContactName { get; set; }

        public ICollection<Employee> Employees { get; set; } 
    }
}
