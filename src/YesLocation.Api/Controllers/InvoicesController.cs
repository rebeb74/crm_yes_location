using AutoMapper;
using YesLocation.Application.DTOs.Invoice;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class InvoicesController : BaseController<Invoice, CreateInvoiceDto, InvoiceDto>
{
  public InvoicesController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }
}
