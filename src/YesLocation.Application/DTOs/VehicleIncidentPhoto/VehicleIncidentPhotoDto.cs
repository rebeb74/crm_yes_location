using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.VehicleIncidentPhoto;

public class VehicleIncidentPhotoDto : BaseModelDto
{
  public int VehicleIncidentId { get; set; }
  public string PhotoUrl { get; set; } = string.Empty;
  public string? Caption { get; set; }
}