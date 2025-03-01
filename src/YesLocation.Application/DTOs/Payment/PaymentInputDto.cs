using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace YesLocation.Application.DTOs.Payment;

public class PaymentInputDto : BaseInputModelDto
{
  public decimal? Amount { get; set; }
  public DateTime? PaymentDate { get; set; }
  public PaymentMethod? PaymentMethod { get; set; }
  public string? TransactionReference { get; set; }

  // Relationships
  [Required]
  public required int InvoiceId { get; set; }
}
