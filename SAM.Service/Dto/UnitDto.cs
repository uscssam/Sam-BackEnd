namespace SAM.Services.Dto;
public class UnitDto : BaseDto, IEquatable<UnitDto>
{
    public required string Name { get; set; }
    public required string Street { get; set; }
    public required string Neighborhood { get; set; }
    public required string CEP { get; set; }
    public required int Number { get; set; }
    public required string Phone { get; set; }

    public bool Equals(UnitDto? other)
    {
        return GetHashCode() == other?.GetHashCode();
    }

    public override int GetHashCode() => (Id, Name, Street, Neighborhood, CEP, Number, Phone).GetHashCode();
}
