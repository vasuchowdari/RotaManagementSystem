using RMS.AppServiceLayer.Base.Dto;
using RMS.AppServiceLayer.SystemAccessRoles.Dto;

namespace RMS.AppServiceLayer.Users.Dto
{
    public class UserDto : BaseDto
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

        public SystemAccessRoleDto SystemAccessRoleDto { get; set; }
    }
}
