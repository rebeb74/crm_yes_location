namespace YesLocation.Application.DTOs
{
  public partial class UserLoginConfirmationDto
  {
    public byte[] PasswordHash { get; set; } = new byte[0];
    public byte[] PasswordSalt { get; set; } = new byte[0];

  }
}