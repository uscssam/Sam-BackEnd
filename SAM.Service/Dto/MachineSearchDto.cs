using SAM.Entities.Enum;

namespace SAM.Services.Dto;
public class MachineSearchDto : BaseDto
{
    public string? Name { get; set; }
    public MachineStatusEnum? Status { get; set; }
    public DateTime? LastMaintenance { get; set; }
    public int? IdUnit { get; set; }
}
