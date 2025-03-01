using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class VehicleInspection : BaseModel
{
  public required int BookingId { get; set; }
  public required virtual Booking Booking { get; set; }
  public required InspectionType Type { get; set; }
  public DateTime InspectionDate { get; set; }
  public string? Notes { get; set; }
  public string? SignatureUrl { get; set; }
  public int? InspectedByUserId { get; set; }
  public virtual User? InspectedByUser { get; set; }

  // Nouvelles propriétés
  public int Mileage { get; set; }              // Kilométrage du véhicule
  public bool VehicleCleaned { get; set; }      // Si le véhicule a été nettoyé
  public FuelLevel FuelLevel { get; set; }      // Niveau de carburant

  // Navigation property
  public virtual ICollection<VehicleIncident> Incidents { get; set; } = [];
}