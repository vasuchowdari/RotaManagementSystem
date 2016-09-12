namespace RMS.AppServiceLayer.Helpers.Dto
{
    public class BaseEmailDto
    {
        public string RecipientAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
