using System.Data.Linq.Mapping;

namespace RMS.Zktime.Classes
{
    [Table(Name="Machines")]
    public class Machine
    {
        [Column(Name="MachineNumber")]
        public int MachineNumber { get; set; }

        [Column(Name="MachineAlias")]
        public string MachineAlias { get; set; }
    }
}
