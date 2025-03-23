using AutoMapper;
using YesLocation.Application.DTOs.MaintenanceRecord;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class MaintenanceRecordsController : BaseController<MaintenanceRecord, MaintenanceRecordInputDto, MaintenanceRecordDto>
{
  public MaintenanceRecordsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }
}
