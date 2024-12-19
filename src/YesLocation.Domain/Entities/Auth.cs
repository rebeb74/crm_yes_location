namespace YesLocation.Domain.Entities;

public class Auth : BaseModel
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
  public byte[]? PasswordHash { get; set; }
  public byte[]? PasswordSalt { get; set; }
}