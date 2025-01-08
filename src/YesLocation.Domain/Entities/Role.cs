namespace YesLocation.Domain.Entities;

public class Role : BaseModel
{
  public string Name { get; set; } = string.Empty;
  public string? Description { get; set; }
  public int Value { get; set; }
  public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
