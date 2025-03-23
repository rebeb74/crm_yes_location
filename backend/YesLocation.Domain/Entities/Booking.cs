using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Booking : BaseModel
{
  public DateTime? PickupDate { get; set; }
  public DateTime? ReturnDate { get; set; }
  public BookingStatus? Status { get; set; }
  public decimal? TotalAmount { get; set; }
  public string? Notes { get; set; }

  // Relationships
  public required int CustomerId { get; set; }
  public required virtual Customer Customer { get; set; }
  public required int VehicleId { get; set; }
  public required virtual Vehicle Vehicle { get; set; }
  public int? PickupLocationId { get; set; }
  public virtual Location? PickupLocation { get; set; }
  public int? ReturnLocationId { get; set; }
  public virtual Location? ReturnLocation { get; set; }
  public int? QuotationId { get; set; }
  public virtual Quotation? Quotation { get; set; }
  public int? InvoiceId { get; set; }
  public virtual Invoice? Invoice { get; set; }

  // Vehicle Inspections
  public virtual ICollection<VehicleInspection> VehicleInspections { get; set; } = [];
}
