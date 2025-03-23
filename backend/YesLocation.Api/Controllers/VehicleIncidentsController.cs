using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.VehicleIncident;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
public class VehicleIncidentsController : BaseController<VehicleIncident, VehicleIncidentInputDto, VehicleIncidentDto>
{
  public VehicleIncidentsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  [HttpGet]
  public override async Task<ActionResult<IEnumerable<VehicleIncidentDto>>> GetAll()
  {
    var incidents = await _context.VehicleIncidents
        .Include(i => i.Photos)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<VehicleIncidentDto>>(incidents));
  }

  [HttpGet("{id}")]
  public override async Task<ActionResult<VehicleIncidentDto>> GetById(int id)
  {
    var incident = await _context.VehicleIncidents
        .Include(i => i.Photos)
        .FirstOrDefaultAsync(i => i.Id == id);

    if (incident == null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<VehicleIncidentDto>(incident));
  }

  // Get incidents for a specific inspection
  [HttpGet("ByInspection/{inspectionId}")]
  public async Task<ActionResult<IEnumerable<VehicleIncidentDto>>> GetByInspection(int inspectionId)
  {
    var inspection = await _context.VehicleInspections.FindAsync(inspectionId);
    if (inspection == null)
    {
      return NotFound("Inspection not found");
    }

    var incidents = await _context.VehicleIncidents
        .Include(i => i.Photos)
        .Where(i => i.VehicleInspectionId == inspectionId)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<VehicleIncidentDto>>(incidents));
  }

  // Get incidents by part location
  [HttpGet("ByPartLocation/{inspectionId}/{partLocation}")]
  public async Task<ActionResult<IEnumerable<VehicleIncidentDto>>> GetByPartLocation(int inspectionId, VehiclePartLocation partLocation)
  {
    var incidents = await _context.VehicleIncidents
        .Include(i => i.Photos)
        .Where(i => i.VehicleInspectionId == inspectionId && i.PartLocation == partLocation)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<VehicleIncidentDto>>(incidents));
  }

  // Create incident with custom validation
  [HttpPost]
  public override async Task<ActionResult<VehicleIncidentDto>> Create(VehicleIncidentInputDto dto)
  {
    // Verify inspection exists
    var inspection = await _context.VehicleInspections.FindAsync(dto.VehicleInspectionId);
    if (inspection == null)
    {
      return NotFound("Inspection not found");
    }

    return await base.Create(dto);
  }
}