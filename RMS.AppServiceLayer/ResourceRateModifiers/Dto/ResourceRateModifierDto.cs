using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.ResourceRateModifiers.Dto
{
    public class ResourceRateModifierDto : BaseDto
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public long ShiftTypeId { get; set; }
        public long ResourceId { get; set; }
    }
}
