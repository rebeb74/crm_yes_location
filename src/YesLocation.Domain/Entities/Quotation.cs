using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Quotation : BaseModel
{
  public int CustomerId { get; set; }
  public virtual Customer Customer { get; set; } = null!;
  public int VehicleId { get; set; }
  public virtual Vehicle Vehicle { get; set; } = null!;
  public DateTime ValidUntil { get; set; }
  public decimal Amount { get; set; }
  public QuotationStatus Status { get; set; }
  public virtual Booking? Booking { get; set; }
}