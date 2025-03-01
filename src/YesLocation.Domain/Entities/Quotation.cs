using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Quotation : BaseModel
{
  public DateTime? ValidUntil { get; set; }
  public decimal? Amount { get; set; }
  public QuotationStatus? Status { get; set; }

  // Relationships
  public virtual Booking? Booking { get; set; }
  public required int CustomerId { get; set; }
  public required virtual Customer Customer { get; set; }
  public required int VehicleId { get; set; }
  public required virtual Vehicle Vehicle { get; set; }
}