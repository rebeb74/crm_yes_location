namespace YesLocation.Application.DTOs.User
{
  public partial class UserLogged
  {
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Token { get; set; }
  }
}