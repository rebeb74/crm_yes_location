using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace YesLocation.Application.DTOs.Invoice;

public class InvoiceInputDto : BaseInputModelDto
{
  [Required]
  public required int BookingId { get; set; }
  public decimal? Amount { get; set; }
  public InvoiceStatus? Status { get; set; }
  public DateTime? IssueDate { get; set; }
  public DateTime? DueDate { get; set; }
  public string? InvoiceNumber { get; set; }
}
