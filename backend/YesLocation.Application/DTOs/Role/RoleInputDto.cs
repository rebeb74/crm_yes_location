
using YesLocation.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;
namespace YesLocation.Application.DTOs.Role;

public class RoleInputDto : BaseInputModelDto
{
  [Required]
  public required string Name { get; set; }
  public int Value { get; set; } = 0;
  public string? Description { get; set; }
}
