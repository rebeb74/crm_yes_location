using YesLocation.Domain.Enums;
using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.Vehicle;

public class VehicleDto : BaseModelDto
{
  public string? Brand { get; set; }
  public string? Model { get; set; }
  public string? RegistrationNumber { get; set; }
  public int? Year { get; set; }
  public VehicleType? Type { get; set; }
  public VehicleCategory? Category { get; set; }
  public string? FuelType { get; set; }
  public string? Transmission { get; set; }
  public int? Mileage { get; set; }
  public VehicleStatus? Status { get; set; }
}
