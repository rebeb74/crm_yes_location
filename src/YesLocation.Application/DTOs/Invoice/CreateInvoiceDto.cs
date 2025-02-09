using YesLocation.Domain.Enums;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Application.DTOs.Invoice;

public class CreateInvoiceDto : IBaseDto
{
  public int? Id { get; set; }
  public int BookingId { get; set; }
  public double Amount { get; set; }
  public InvoiceStatus Status { get; set; }
  public DateTime DueDate { get; set; }
}
