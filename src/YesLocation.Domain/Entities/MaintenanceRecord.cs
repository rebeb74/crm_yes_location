using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class MaintenanceRecord : BaseModel
{
  public int VehicleId { get; set; }
  public virtual Vehicle Vehicle { get; set; } = null!;
  public DateTime ServiceDate { get; set; }
  public string Description { get; set; } = string.Empty;
  public decimal Cost { get; set; }
  public int Mileage { get; set; }
  public string? ServiceProvider { get; set; }
  public MaintenanceType Type { get; set; }
}
