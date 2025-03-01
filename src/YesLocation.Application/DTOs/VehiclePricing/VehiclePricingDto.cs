using YesLocation.Application.DTOs.Common;
using YesLocation.Application.DTOs.Vehicle;
using YesLocation.Application.DTOs.Season;
using YesLocation.Application.DTOs.DurationTier;

namespace YesLocation.Application.DTOs.VehiclePricing;

public class VehiclePricingDto : BaseModelDto
{
  public required int VehicleId { get; set; }
  public VehicleDto? Vehicle { get; set; }

  public required int SeasonId { get; set; }
  public SeasonDto? Season { get; set; }

  public required int DurationTierId { get; set; }
  public DurationTierDto? DurationTier { get; set; }

  public required decimal DailyRate { get; set; }
}