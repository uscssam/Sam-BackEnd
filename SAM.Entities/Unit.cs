namespace SAM.Entities
{
    public class Unit : BaseEntity
    {
        public required string Name { get; set; }
        public required string Street { get; set; }
        public required string Neighborhood { get; set; }
        public required string CEP { get; set; }
        public required int Number { get; set; }
        public required string Phone { get; set; }
    }
}
