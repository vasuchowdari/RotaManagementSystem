namespace RMS.WebAPI.ViewModels
{
    public class ShiftTemplateSearchCriteriaModel
    {
        public long ResourceId { get; set; }
        public long SiteId { get; set; }
        public long? SubSiteId { get; set; }
    }
}