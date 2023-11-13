namespace SAM.Entities
{
    public class Unit : BaseEntity
    {
        public string? Name { get; set; }
        public string? Road { get; set; }
        public string? Neighborhood { get; set; }
        public string? CEP { get; set; }
        public int Number { get; set; }
        public string? Phone { get; set; }
    }
}
