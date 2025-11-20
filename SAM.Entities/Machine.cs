using SAM.Entities.Enum;

namespace SAM.Entities
{
    public class Machine : BaseEntity
    {
        //comentário
        public required string Name { get; set; }
        public required MachineStatusEnum Status { get; set; }
        public DateTime? LastMaintenance { get; set; }
        public required int IdUnit { get; set; }
    }
}
