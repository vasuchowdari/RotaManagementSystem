using System;
using System.Collections.Generic;
using RMS.AppServiceLayer.ShiftProfiles.Dto;

namespace RMS.AppServiceLayer.Zktime.Events
{
    public class ShiftProfileEventArgs : EventArgs
    {
        public ICollection<ShiftProfileDto> ShiftProfileDtos { get; set; } 
    }
}
