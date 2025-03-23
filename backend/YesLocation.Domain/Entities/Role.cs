namespace YesLocation.Domain.Entities;

public class Role : BaseModel
{
  public required string Name { get; set; }
  public required int Value { get; set; }
  public string? Description { get; set; }
  public virtual ICollection<UserRole> UserRoles { get; set; } = [];
}
