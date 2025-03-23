using YesLocation.Application.DTOs.Booking;
using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Invoice;

public class InvoiceDto : BaseModelDto
{
  public string? InvoiceNumber { get; set; } // TODO: Handle automatic generation
  public DateTime? IssueDate { get; set; }
  public DateTime? DueDate { get; set; }
  public decimal? Amount { get; set; }
  public decimal? PaidAmount { get; set; }
  public InvoiceStatus? Status { get; set; }

  // Relationships
  public int BookingId { get; set; }
}
