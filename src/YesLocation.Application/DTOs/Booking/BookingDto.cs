using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Booking;

public class BookingDto : BaseModelDto
{
  public DateTime? PickupDate { get; set; }
  public DateTime? ReturnDate { get; set; }
  public BookingStatus? Status { get; set; }
  public decimal? TotalAmount { get; set; }
  public string? Notes { get; set; }

  // Relationships
  public required int CustomerId { get; set; }
  public required int VehicleId { get; set; }
  public int? PickupLocationId { get; set; }
  public int? ReturnLocationId { get; set; }
  public int? QuotationId { get; set; }
  public int? InvoiceId { get; set; }
}
