using YesLocation.Application.DTOs.Common;
using YesLocation.Application.DTOs.VehicleIncidentPhoto;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.VehicleIncident;

public class VehicleIncidentDto : BaseModelDto
{
  public int VehicleInspectionId { get; set; }
  public VehiclePartLocation PartLocation { get; set; }
  public IncidentType Type { get; set; }
  public string? Description { get; set; }

  // Navigation property (simplifiée)
  public List<VehicleIncidentPhotoDto>? Photos { get; set; }
}