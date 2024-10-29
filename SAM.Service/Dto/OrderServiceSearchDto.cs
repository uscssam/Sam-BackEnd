using SAM.Entities.Enum;

namespace SAM.Services.Dto;
public class OrderServiceSearchDto : BaseDto
{
    public string? Description { get; set; }
    public OrderServiceStatusEnum? Status { get; set; }
    public DateTime? Opening { get; set; }
    public DateTime? Closed { get; set; }
    public int? IdMachine { get; set; }
    public int? IdTechnician { get; set; }
    public int? CreatedBy { get; set; }
}
