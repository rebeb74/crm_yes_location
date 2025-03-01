using YesLocation.Domain.Interfaces;

namespace YesLocation.Application.DTOs.Common;

public abstract class BaseModelDto : IBaseModelDto
{
  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public int? CreatedBy { get; set; }
  public int? UpdatedBy { get; set; }
}
