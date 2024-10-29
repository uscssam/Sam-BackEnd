using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SAM.Entities;

[ExcludeFromCodeCoverage]
public abstract class BaseEntity
{
    public int Id { get; set; }
    [JsonIgnore]
    public DateTime? DeletedAt { get; set; }
}
