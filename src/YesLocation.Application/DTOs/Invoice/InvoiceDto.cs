using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Invoice;

public class InvoiceDto
{
  public int Id { get; set; }
  public int BookingId { get; set; }
  public double Amount { get; set; }
  public InvoiceStatus Status { get; set; }
  public DateTime DueDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
