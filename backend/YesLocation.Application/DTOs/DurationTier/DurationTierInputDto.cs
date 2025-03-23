using System.ComponentModel.DataAnnotations;
using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.DurationTier;

public class DurationTierInputDto : BaseInputModelDto
{
  [Required]
  [StringLength(50)]
  public required string Name { get; set; }

  [Required]
  [Range(1, 365)]
  public required int MinDays { get; set; }

  [Range(1, 365)]
  public int? MaxDays { get; set; }
}