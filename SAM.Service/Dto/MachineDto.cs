using SAM.Entities.Enum;

namespace SAM.Services.Dto;
public class MachineDto : BaseDto, IEquatable<MachineDto>
{
    public required string Name { get; set; }
    public MachineStatusEnum? Status { get; internal set; }
    public DateTime? LastMaintenance { get; internal set; }
    public required int IdUnit { get; set; }

    public bool Equals(MachineDto? other)
    {
        return GetHashCode() == other?.GetHashCode();
    }

    public override int GetHashCode() => (Id, IdUnit, Name, Status, LastMaintenance).GetHashCode();
}
