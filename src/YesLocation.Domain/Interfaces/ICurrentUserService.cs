namespace YesLocation.Domain.Interfaces;

public interface ICurrentUserService
{
  int? Id { get; set; }
  string? Username { get; set; }
  string? Email { get; set; }
  string? FirstName { get; set; }
  string? LastName { get; set; }
  // string Role { get; set; }
}