namespace YesLocation.Domain.Entities;

public class VehicleIncidentPhoto : BaseModel
{
  public required int VehicleIncidentId { get; set; }
  public required virtual VehicleIncident VehicleIncident { get; set; }
  public required string PhotoUrl { get; set; }
  public string? Caption { get; set; }
}