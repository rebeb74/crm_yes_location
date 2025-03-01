using YesLocation.Domain.Enums;
using YesLocation.Application.DTOs.Common;
using System.ComponentModel.DataAnnotations;
namespace YesLocation.Application.DTOs.Booking;

public class BookingInputDto : BaseInputModelDto
{
  public DateTime? PickupDate { get; set; }
  public DateTime? ReturnDate { get; set; }
  public BookingStatus? Status { get; set; }
  public string? Notes { get; set; }
  public decimal? TotalAmount { get; set; }

  // Relationships
  [Required]
  public required int CustomerId { get; set; }
  [Required]
  public required int VehicleId { get; set; }
  public int? PickupLocationId { get; set; }
  public int? ReturnLocationId { get; set; }
  public int? QuotationId { get; set; }
  public int? InvoiceId { get; set; }
}
