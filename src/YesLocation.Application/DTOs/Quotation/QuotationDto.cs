using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Quotation;

public class QuotationDto : BaseModelDto
{
  public DateTime? ValidUntil { get; set; }
  public decimal? Amount { get; set; }
  public QuotationStatus? Status { get; set; }

  // Relationships
  public required int CustomerId { get; set; }
  public required int VehicleId { get; set; }
}
