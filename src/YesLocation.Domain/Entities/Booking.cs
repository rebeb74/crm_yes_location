using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Booking : BaseModel
{
  public DateTime PickupDate { get; set; }
  public DateTime ReturnDate { get; set; }
  public BookingStatus Status { get; set; }
  public decimal TotalAmount { get; set; }
  public string? Notes { get; set; }

  // Relationships
  public int CustomerId { get; set; }
  public virtual Customer Customer { get; set; } = null!;
  public int VehicleId { get; set; }
  public virtual Vehicle Vehicle { get; set; } = null!;
  public int PickupLocationId { get; set; }
  public virtual Location PickupLocation { get; set; } = null!;
  public int ReturnLocationId { get; set; }
  public virtual Location ReturnLocation { get; set; } = null!;
  public int? QuotationId { get; set; }
  public virtual Quotation? Quotation { get; set; }
  public int? InvoiceId { get; set; }
  public virtual Invoice? Invoice { get; set; }
}
