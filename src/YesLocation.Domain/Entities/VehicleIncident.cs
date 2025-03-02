using System.ComponentModel.DataAnnotations.Schema;
using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class VehicleIncident : BaseModel
{
  public required VehiclePartLocation PartLocation { get; set; }
  public required IncidentType Type { get; set; }
  public string? Description { get; set; }

  // Relationships
  public required int VehicleInspectionId { get; set; }
  public required virtual VehicleInspection VehicleInspection { get; set; }

  // Navigation property
  public virtual ICollection<VehicleIncidentPhoto> Photos { get; set; } = [];
}