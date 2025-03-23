using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Season;

public class SeasonDto : BaseModelDto
{
  public required string Name { get; set; }
  public required DateTime StartDate { get; set; }
  public required DateTime EndDate { get; set; }
  public required int Year { get; set; }
  public required SeasonType Type { get; set; }
}