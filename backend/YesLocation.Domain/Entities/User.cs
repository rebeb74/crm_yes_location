namespace YesLocation.Domain.Entities;

public class User : BaseModel
{
  private string? _Username;
  public string? Username
  {
    get
    {
      return _Username != null ? char.ToUpper(_Username[0]) + _Username[1..] : null;
    }
    set
    {
      _Username = value?.ToLower();
    }
  }
  private string? _Email;
  public string? Email
  {
    get { return _Email; }
    set
    {
      _Email = value?.ToLower();
    }
  }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public virtual ICollection<UserRole> UserRoles { get; set; } = [];
  public virtual Auth? Auth { get; set; }
}
