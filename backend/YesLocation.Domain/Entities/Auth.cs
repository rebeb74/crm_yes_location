namespace YesLocation.Domain.Entities;

public class Auth : BaseModel
{
  public int UserId { get; set; }
  public User User { get; set; } = null!;
  public byte[]? PasswordHash { get; set; }
  public byte[]? PasswordSalt { get; set; }
}