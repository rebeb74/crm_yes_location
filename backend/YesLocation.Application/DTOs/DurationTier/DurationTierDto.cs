using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.DurationTier;

public class DurationTierDto : BaseModelDto
{
  public required string Name { get; set; }
  public required int MinDays { get; set; }
  public int? MaxDays { get; set; }
}