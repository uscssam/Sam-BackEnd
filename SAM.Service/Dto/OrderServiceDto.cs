using SAM.Entities.Enum;

namespace SAM.Services.Dto;
public class OrderServiceDto : BaseDto, IEquatable<OrderServiceDto>
{
    public string? Description { get; set; }
    public OrderServiceStatusEnum? Status { get; set; }
    public DateTime? Opening { get; internal set; }
    public DateTime? Closed { get; internal set; }
    public int? IdMachine { get; set; }
    public int? IdTechnician { get; set; }
    public int? CreatedBy { get; internal set; }

    public bool Equals(OrderServiceDto? other)
    {
        return GetHashCode() == other?.GetHashCode();
    }

    public override int GetHashCode() => (Id, IdMachine, Description, Status, Opening, Closed, IdTechnician, CreatedBy).GetHashCode();
}
