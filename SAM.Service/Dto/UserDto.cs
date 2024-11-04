using SAM.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SAM.Services.Dto;
[ExcludeFromCodeCoverage]
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
    [MaxLength(50, ErrorMessage = "O nome de usuário deve ter no máximo 50 caracteres")]
    public required string UserName { get; set; }
    [MaxLength(50, ErrorMessage = "O nome completo deve ter no máximo 50 caracteres")]
    public required string Fullname { get; set; }
    [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
    public required string Email { get; set; }
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Número de telefone inválido")]
    public required string Phone { get; set; }
    public required LevelEnum Level { get; set; }
    public TechnicianTypeEnum? Speciality { get; set; }
    [RegularExpression(@"^.{4,20}$", ErrorMessage = "A senha deve ter entre 4 e 20 caracteres")]
    public string? Password { get; set; }

    public bool Equals(UserDto? other)
    {
        return GetHashCode() == other?.GetHashCode();
    }

    public override int GetHashCode() => (Id, UserName, Fullname, Email, Phone, Level, Speciality, Password).GetHashCode();
}
