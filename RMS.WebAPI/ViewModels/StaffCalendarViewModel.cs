using System.Collections.Generic;
using RMS.WebAPI.Models;

namespace RMS.WebAPI.ViewModels
{
    public class StaffCalendarViewModel
    {
        public ICollection<ShiftModel> ShiftModels { get; set; }
        public ICollection<LeaveRequestModel> LeaveRequestModels { get; set; } 
        // LEAVE TYPES
        // TRAINING TYPES
    }
}