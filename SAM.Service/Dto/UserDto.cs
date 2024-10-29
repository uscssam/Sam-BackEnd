using SAM.Entities.Enum;

namespace SAM.Services.Dto;

public class UserReturnDto : BaseDto, IEquatable<UserReturnDto>
{
    public UserReturnDto(UserDto user)
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

    public bool Equals(UserReturnDto? other)
    {
        return GetHashCode() == other?.GetHashCode();
    }

    public override int GetHashCode() => (Id, UserName, Fullname, Email, Phone, Level, Speciality).GetHashCode();
}

public class UserDto : BaseDto, IEquatable<UserDto>
{
    public required string UserName { get; set; }
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required LevelEnum Level { get; set; }
    public TechnicianTypeEnum? Speciality { get; set; }
    public string? Password { get; set; }

    public bool Equals(UserDto? other)
    {
        return GetHashCode() == other?.GetHashCode();
    }

    public override int GetHashCode() => (Id, UserName, Fullname, Email, Phone, Level, Speciality, Password).GetHashCode();
}
