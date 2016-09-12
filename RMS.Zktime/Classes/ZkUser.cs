using System.Data.Linq.Mapping;

namespace RMS.Zktime.Classes
{
    [Table(Name="USERINFO")]
    public class ZkUser
    {
        [Column(Name="USERID")]
        public int UserId { get; set; }

        [Column(Name="BADGENUMBER")]
        public string BadgeNumber { get; set; }

        [Column(Name="NAME")]
        public string Name { get; set; }
    }
}
