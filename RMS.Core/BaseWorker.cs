using System.Collections.Generic;

namespace RMS.Core
{
    public class BaseWorker : BaseEntity
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public long GenderId { get; set; }
        public long BaseSiteId { get; set; }
        public long? BaseSubSiteId { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public ICollection<PersonnelLeaveProfile> LeaveProfiles { get; set; } 
    }
}
