namespace RMS.WebAPI.Models
{
    public class UserModel : BaseModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long SystemAccessRoleId { get; set; }
        public bool IsAccountLocked { get; set; }
        public int? ExternalTimeSystemId { get; set; }
        public string PayrollReferenceNumber { get; set; }

        public SystemAccessRoleModel SystemAccessRoleModel { get; set; }
    }
}