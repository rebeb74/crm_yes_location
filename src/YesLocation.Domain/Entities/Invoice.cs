using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Invoice : BaseModel
{
  public string? InvoiceNumber { get; set; }
  public DateTime? IssueDate { get; set; }
  public DateTime? DueDate { get; set; }
  public decimal? Amount { get; set; }
  public decimal? PaidAmount { get; set; } // TODO: Handle automatic sum of payments
  public InvoiceStatus? Status { get; set; }

  // Relationships
  public required int BookingId { get; set; }
  public required virtual Booking Booking { get; set; }
  public virtual ICollection<Payment> Payments { get; set; } = [];

  public void CalculatePaidAmount(IEnumerable<Payment> payments)
  {
    PaidAmount = payments.Sum(p => p.Amount ?? 0);
  }
}
