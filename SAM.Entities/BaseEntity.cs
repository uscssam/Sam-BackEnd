using System.ComponentModel.DataAnnotations.Schema;

namespace SAM.Entities;
public abstract class BaseEntity
{
    [NotMapped]
    public int Id { get; set; }
    public DateTime? DeletedAt { get; set; }
}
