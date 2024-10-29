using SAM.Entities.Enum;

namespace SAM.Entities
{
    public class OrderService : BaseEntity
    {
        public required string Description { get; set; }
        public required OrderServiceStatusEnum Status { get; set; }
        public required DateTime Opening { get; set; }
        public DateTime? Closed { get; set; }
        public required int IdMachine { get; set; }
        public int? IdTechnician { get; set; }
        public required int CreatedBy { get; set; }
    }
}
