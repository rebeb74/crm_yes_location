using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Payment : BaseModel
{
  public decimal? Amount { get; set; }
  public DateTime? PaymentDate { get; set; }
  public PaymentMethod? PaymentMethod { get; set; }
  public string? TransactionReference { get; set; }

  // Relationships
  public required int InvoiceId { get; set; }
  public required virtual Invoice Invoice { get; set; }
}
