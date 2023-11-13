using SAM.Entities.Enum;
using System.Text.Json.Serialization;

namespace SAM.Entities
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        [JsonIgnore]
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public LevelEnum? Level { get; set; }
        public TechnicianTypeEnum? Speciality { get; set; }
        public bool Available { get; set; }
    }
}
