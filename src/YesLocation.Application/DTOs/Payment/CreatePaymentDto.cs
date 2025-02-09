using YesLocation.Domain.Enums;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Application.DTOs.Payment;

public class CreatePaymentDto : IBaseDto
{
  public int? Id { get; set; }
  public int RentalId { get; set; }
  public double Amount { get; set; }
  public PaymentMethod PaymentMethod { get; set; }
  public DateTime PaymentDate { get; set; }
}
