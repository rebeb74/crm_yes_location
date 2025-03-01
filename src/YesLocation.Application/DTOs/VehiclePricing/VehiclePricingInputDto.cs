using System.ComponentModel.DataAnnotations;
using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.VehiclePricing;

public class VehiclePricingInputDto : BaseInputModelDto
{
  [Required]
  public required int VehicleId { get; set; }

  // If not specified or ≤ 0, the default low season will be used
  [Required]
  public required int SeasonId { get; set; }

  [Required]
  public required int DurationTierId { get; set; }

  [Required]
  [Range(0.01, 99999.99)]
  public required decimal DailyRate { get; set; }
}