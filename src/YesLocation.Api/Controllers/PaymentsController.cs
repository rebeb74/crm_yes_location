using AutoMapper;
using YesLocation.Application.DTOs.Payment;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class PaymentsController : BaseController<Payment, CreatePaymentDto, CreatePaymentDto>
{
  public PaymentsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }
}
