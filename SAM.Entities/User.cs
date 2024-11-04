using SAM.Entities.Enum;
using SAM.Entities;
using SAM.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class UserReturn : BaseEntity, ICurrentUser
{
    public UserReturn() { }
    public UserReturn(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Fullname = user.Fullname;
        Email = user.Email;
        Phone = user.Phone;
        Level = user.Level;
        Speciality = user.Speciality;
    }

    public string? UserName { get; set; }
    public string? Fullname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public LevelEnum? Level { get; set; }
    public TechnicianTypeEnum? Speciality { get; set; }
}

public class User : BaseEntity
{
    public required string UserName { get; set; }
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required LevelEnum Level { get; set; }
    public TechnicianTypeEnum? Speciality { get; set; }
    public string Password { get; set; } = string.Empty;
}
