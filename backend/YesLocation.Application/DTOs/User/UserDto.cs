using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.User;
public class UserDto : BaseModelDto
{
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public required string Email { get; set; }
  public required string Username { get; set; }
}