using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Invoice : BaseModel
{
  public int BookingId { get; set; }
  public virtual Booking Booking { get; set; } = null!;
  public string InvoiceNumber { get; set; } = string.Empty;
  public DateTime IssueDate { get; set; }
  public DateTime DueDate { get; set; }
  public decimal Amount { get; set; }
  public decimal? PaidAmount { get; set; }
  public InvoiceStatus Status { get; set; }
  public virtual ICollection<Payment> Payments { get; set; } = [];
}
