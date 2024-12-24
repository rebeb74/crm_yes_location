using YesLocation.Domain.Interfaces;

namespace YesLocation.Api.Services;

public class CurrentUserService : ICurrentUserService
{
  public int? Id { get; set; }
  public string? Username { get; set; }
  public string? Email { get; set; }
  // public string Role { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
}