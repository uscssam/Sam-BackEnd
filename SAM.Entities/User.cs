using SAM.Entities.Enum;

namespace SAM.Entities
{
    public class User
    {
        public int IdUser { get; private set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public LevelEnum? Level { get; set; }
        public TechnicianTypeEnum? Speciality { get; set; } 
        /// <summary>
        /// Para aqueles que estão em atendimento
        /// </summary>
        public bool Available { get; set; }
    }
}
