using AutoMapper;
using YesLocation.Application.DTOs.Booking;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class BookingsController : BaseController<Booking, BookingInputDto, BookingDto>
{
  public BookingsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }
}
