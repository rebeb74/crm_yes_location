using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.MaintenanceRecord;

public class MaintenanceRecordDto : BaseModelDto
{
  public DateTime? MaintenanceDate { get; set; }
  public string? Description { get; set; }
  public decimal? Cost { get; set; }
  public int? Mileage { get; set; }
  public string? ServiceProvider { get; set; }
  public MaintenanceType? Type { get; set; }

  // Relationships
  public required int VehicleId { get; set; }
}
