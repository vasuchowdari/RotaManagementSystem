using RMS.AppServiceLayer.Base.Dto;

namespace RMS.AppServiceLayer.Resources.Dto
{
    public class ResourceDto : BaseDto
    {
        public string Name { get; set; }
        public decimal BaseRate { get; set; }
    }
}
