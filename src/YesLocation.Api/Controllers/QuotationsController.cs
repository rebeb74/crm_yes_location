using AutoMapper;
using YesLocation.Application.DTOs.Quotation;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class QuotationsController : BaseController<Quotation, QuotationInputDto, QuotationDto>
{
  public QuotationsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }
}