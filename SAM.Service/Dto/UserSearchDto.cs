using SAM.Entities.Enum;
using System.Diagnostics.CodeAnalysis;

namespace SAM.Services.Dto;
[ExcludeFromCodeCoverage]
public class UserSearchDto : BaseDto
{
    public string? UserName { get; set; }
    public string? Fullname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public LevelEnum? Level { get; set; }
    public TechnicianTypeEnum? Speciality { get; set; }
}
