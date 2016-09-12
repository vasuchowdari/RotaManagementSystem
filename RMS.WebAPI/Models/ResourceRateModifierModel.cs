namespace RMS.WebAPI.Models
{
    public class ResourceRateModifierModel : BaseModel
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public long ShiftTypeId { get; set; }
        public long ResourceId { get; set; }
    }
}