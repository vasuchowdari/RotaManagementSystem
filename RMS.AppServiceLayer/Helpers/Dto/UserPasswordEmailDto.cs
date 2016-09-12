namespace RMS.AppServiceLayer.Helpers.Dto
{
    public class UserPasswordEmailDto : BaseEmailDto
    {
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Login { get; set; }
    }
}
