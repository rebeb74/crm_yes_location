using System.ComponentModel.DataAnnotations;
using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.VehicleIncidentPhoto;

public class VehicleIncidentPhotoInputDto : BaseInputModelDto
{
  [Required]
  public int VehicleIncidentId { get; set; }

  [Required]
  public string PhotoUrl { get; set; } = string.Empty;

  public string? Caption { get; set; }
}