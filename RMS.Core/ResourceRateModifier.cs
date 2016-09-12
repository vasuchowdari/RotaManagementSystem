namespace RMS.Core
{
    public class ResourceRateModifier : BaseEntity
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public long ShiftTypeId { get; set; }
        public long ResourceId { get; set; }
        
        //need to add in
        // SiteId as Baldock have different rates to the other
    }
}
