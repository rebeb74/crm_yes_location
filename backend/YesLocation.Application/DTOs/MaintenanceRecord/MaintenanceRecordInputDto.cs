using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace YesLocation.Application.DTOs.MaintenanceRecord;

public class MaintenanceRecordInputDto : BaseInputModelDto
{
  public DateTime? MaintenanceDate { get; set; }
  public string? Description { get; set; }
  public decimal? Cost { get; set; }
  public int? Mileage { get; set; }
  public string? ServiceProvider { get; set; }
  public MaintenanceType? Type { get; set; }

  // Relationships
  [Required]
  public required int VehicleId { get; set; }
}
