using System.ComponentModel.DataAnnotations;
using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.VehicleIncident;

public class VehicleIncidentInputDto : BaseInputModelDto
{
  [Required]
  public int VehicleInspectionId { get; set; }

  [Required]
  public VehiclePartLocation PartLocation { get; set; }

  [Required]
  public IncidentType Type { get; set; }

  public string? Description { get; set; }
}