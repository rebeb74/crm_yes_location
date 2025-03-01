using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Payment;

public class PaymentDto : BaseModelDto
{
  public decimal? Amount { get; set; }
  public DateTime? PaymentDate { get; set; }
  public PaymentMethod? PaymentMethod { get; set; }
  public string? TransactionReference { get; set; }

  // Relationships
  public required int InvoiceId { get; set; }
}
