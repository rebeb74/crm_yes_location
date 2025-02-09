using AutoMapper;
using YesLocation.Application.DTOs.Vehicle;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class VehiclesController : BaseController<Vehicle, CreateVehicleDto, VehicleDto>
{
  public VehiclesController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }
}
