using System.ComponentModel.DataAnnotations;
using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.VehicleInspection;

public class VehicleInspectionInputDto : BaseInputModelDto
{
  [Required]
  public int BookingId { get; set; }

  [Required]
  public InspectionType Type { get; set; }

  [Required]
  public DateTime InspectionDate { get; set; }

  public string? Notes { get; set; }

  public string? SignatureUrl { get; set; }

  public int? InspectedByUserId { get; set; }

  [Required]
  public int Mileage { get; set; }

  [Required]
  public bool VehicleCleaned { get; set; }

  [Required]
  public FuelLevel FuelLevel { get; set; }
}