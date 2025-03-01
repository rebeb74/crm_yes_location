using AutoMapper;
using YesLocation.Application.DTOs.Customer;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class CustomersController : BaseController<Customer, CustomerInputDto, CustomerDto>
{
  public CustomersController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }
}
