using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Vehicle;

public class VehicleDto
{
  public int Id { get; set; }
  public string Brand { get; set; } = string.Empty;
  public string Model { get; set; } = string.Empty;
  public string RegistrationNumber { get; set; } = string.Empty;
  public int Year { get; set; }
  public VehicleType Type { get; set; }
  public VehicleCategory Category { get; set; }
  public double DailyRate { get; set; }
  public string? FuelType { get; set; }
  public string? Transmission { get; set; }
  public int Mileage { get; set; }
  public VehicleStatus Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
