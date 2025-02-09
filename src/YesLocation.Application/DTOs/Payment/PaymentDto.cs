using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Payment;

public class PaymentDto
{
  public int Id { get; set; }
  public int RentalId { get; set; }
  public double Amount { get; set; }
  public PaymentMethod PaymentMethod { get; set; }
  public DateTime PaymentDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
