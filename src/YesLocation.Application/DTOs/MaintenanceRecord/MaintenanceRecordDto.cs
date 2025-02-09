namespace YesLocation.Application.DTOs.MaintenanceRecord;

public class MaintenanceRecordDto
{
  public int Id { get; set; }
  public int VehicleId { get; set; }
  public string Description { get; set; } = string.Empty;
  public double Cost { get; set; }
  public DateTime MaintenanceDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
