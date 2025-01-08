namespace YesLocation.Application.DTOs.Role
{
  public class UserRoleDto
  {
    public int UserId { get; set; }
    public List<int> RoleIds { get; set; } = new();
  }
}