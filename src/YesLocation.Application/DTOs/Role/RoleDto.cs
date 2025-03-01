using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.Role;

public class RoleDto : BaseModelDto
{
  public required string Name { get; set; }
  public required int Value { get; set; }
  public string? Description { get; set; }
}
