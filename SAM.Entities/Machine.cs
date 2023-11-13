namespace SAM.Entities
{
    public class Machine : BaseEntity
    {
        public string? Name { get; set; }
        public bool Status { get; set; }
        public DateTime LastMaintenance { get; set; }
        public DateTime Preventive { get; set; }
    }
}
