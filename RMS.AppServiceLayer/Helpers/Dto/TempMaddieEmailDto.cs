namespace RMS.AppServiceLayer.Helpers.Dto
{
    public class TempMaddieEmailDto : BaseEmailDto
    {
        public string ShiftStartDateTime { get; set; }
        public string ShiftEndDateTime { get; set; }
        public string ShiftLocation { get; set; }
        public string ShiftNewStaffMember { get; set; }
        public string ShiftOldStaffMember { get; set; }
        public string ResourceTypeName { get; set; }
    }
}
