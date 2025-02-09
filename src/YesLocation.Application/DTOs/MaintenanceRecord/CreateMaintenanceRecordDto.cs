using YesLocation.Domain.Interfaces;

namespace YesLocation.Application.DTOs.MaintenanceRecord;

public class CreateMaintenanceRecordDto : IBaseDto
{
  public int? Id { get; set; }
  public int VehicleId { get; set; }
  public string Description { get; set; } = string.Empty;
  public double Cost { get; set; }
  public DateTime MaintenanceDate { get; set; }
}
