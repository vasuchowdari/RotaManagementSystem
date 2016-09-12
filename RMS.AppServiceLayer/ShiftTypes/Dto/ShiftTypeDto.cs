using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.ShiftTypes.Dto
{
    public class ShiftTypeDto : BaseDto
    {
        public string Name { get; set; }
        public bool IsOvernight { get; set; }
    }
}
