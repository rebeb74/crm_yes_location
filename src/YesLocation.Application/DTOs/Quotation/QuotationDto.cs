using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Quotation;

public class QuotationDto
{
  public int Id { get; set; }
  public int CustomerId { get; set; }
  public int VehicleId { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public double TotalAmount { get; set; }
  public QuotationStatus Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
