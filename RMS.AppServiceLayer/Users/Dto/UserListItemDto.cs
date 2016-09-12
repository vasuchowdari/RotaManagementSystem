using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.AppServiceLayer.Users.Dto
{
    public class UserListItemDto
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
