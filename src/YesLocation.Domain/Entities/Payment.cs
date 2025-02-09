using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Payment : BaseModel
{
  public int InvoiceId { get; set; }
  public virtual Invoice Invoice { get; set; } = null!;
  public decimal Amount { get; set; }
  public DateTime PaymentDate { get; set; }
  public PaymentMethod PaymentMethod { get; set; }
  public string? TransactionReference { get; set; }
}
