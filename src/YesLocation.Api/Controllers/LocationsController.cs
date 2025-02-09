using AutoMapper;
using YesLocation.Application.DTOs.Location;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class LocationsController : BaseController<Location, CreateLocationDto, LocationDto>
{
  public LocationsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }
}
