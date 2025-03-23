using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.VehicleIncidentPhoto;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
public class VehicleIncidentPhotosController : BaseController<VehicleIncidentPhoto, VehicleIncidentPhotoInputDto, VehicleIncidentPhotoDto>
{
  public VehicleIncidentPhotosController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  // Get photos for a specific incident
  [HttpGet("ByIncident/{incidentId}")]
  public async Task<ActionResult<IEnumerable<VehicleIncidentPhotoDto>>> GetByIncident(int incidentId)
  {
    var incident = await _context.VehicleIncidents.FindAsync(incidentId);
    if (incident == null)
    {
      return NotFound("Incident not found");
    }

    var photos = await _context.VehicleIncidentPhotos
        .Where(p => p.VehicleIncidentId == incidentId)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<VehicleIncidentPhotoDto>>(photos));
  }

  // Create photo with custom validation
  [HttpPost]
  public override async Task<ActionResult<VehicleIncidentPhotoDto>> Create(VehicleIncidentPhotoInputDto dto)
  {
    // Verify incident exists
    var incident = await _context.VehicleIncidents.FindAsync(dto.VehicleIncidentId);
    if (incident == null)
    {
      return NotFound("Incident not found");
    }

    return await base.Create(dto);
  }

  // TODO: Implement photo upload endpoint if needed
  // This would handle multipart form data to upload the actual image files
}