using System.Diagnostics.CodeAnalysis;

namespace SAM.Services.Dto;
[ExcludeFromCodeCoverage]
public class UnitSearchDto :  BaseDto
{
    public  string? Name { get; set; }
    public string? Street { get; set; }
    public string? Neighborhood { get; set; }
    public string? CEP { get; set; }
    public int? Number { get; set; }
    public string? Phone { get; set; }
}
