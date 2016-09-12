using System;
using System.Collections.Generic;

namespace RMS.Core
{
    public class Employee : BaseWorker
    {
        public long CompanyId { get; set; }
        public long EmployeeTypeId { get; set; }

        public Company Company { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public ICollection<Contract> Contracts { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? LeaveDate { get; set; }
    }
}
