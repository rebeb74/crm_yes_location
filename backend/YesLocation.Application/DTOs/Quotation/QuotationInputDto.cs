using YesLocation.Application.DTOs.Common;
using YesLocation.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace YesLocation.Application.DTOs.Quotation;

public class QuotationInputDto : BaseInputModelDto
{
  public decimal? Amount { get; set; }
  public QuotationStatus? Status { get; set; }
  public DateTime? ValidUntil { get; set; }

  // Relationships
  [Required]
  public required int CustomerId { get; set; }
  [Required]
  public required int VehicleId { get; set; }
}
