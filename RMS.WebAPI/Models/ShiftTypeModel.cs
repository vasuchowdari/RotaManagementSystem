namespace RMS.WebAPI.Models
{
    public class ShiftTypeModel : BaseModel
    {
        public string Name { get; set; }
        public bool IsOvernight { get; set; }
    }
}