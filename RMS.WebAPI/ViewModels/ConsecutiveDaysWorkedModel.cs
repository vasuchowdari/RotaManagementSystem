using System;

namespace RMS.WebAPI.ViewModels
{
    public class ConsecutiveDaysWorkedModel
    {
        public long StaffId { get; set; }
        public bool IsEmployee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}