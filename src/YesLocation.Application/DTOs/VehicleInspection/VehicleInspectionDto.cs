using YesLocation.Application.DTOs.Common;
using YesLocation.Application.DTOs.VehicleIncident;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.VehicleInspection;

public class VehicleInspectionDto : BaseModelDto
{
  public int BookingId { get; set; }
  public InspectionType Type { get; set; }
  public DateTime InspectionDate { get; set; }
  public string? Notes { get; set; }
  public string? SignatureUrl { get; set; }
  public int? InspectedByUserId { get; set; }

  // Propriétés additionnelles
  public int Mileage { get; set; }
  public bool VehicleCleaned { get; set; }
  public FuelLevel FuelLevel { get; set; }

  // Navigation property (simplifiée)
  public List<VehicleIncidentDto>? Incidents { get; set; }
}