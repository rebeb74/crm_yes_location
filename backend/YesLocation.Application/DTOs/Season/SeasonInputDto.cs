using System.ComponentModel.DataAnnotations;
using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Season;

public class SeasonInputDto : BaseInputModelDto
{
  [Required]
  [StringLength(50)]
  public required string Name { get; set; }

  [Required]
  public required DateTime StartDate { get; set; }

  [Required]
  public required DateTime EndDate { get; set; }

  [Required]
  [Range(2000, 2100)]
  public required int Year { get; set; }

  [Required]
  public required SeasonType Type { get; set; }
}