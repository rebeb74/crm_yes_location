using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.VehicleInspection;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
public class VehicleInspectionsController : BaseController<VehicleInspection, VehicleInspectionInputDto, VehicleInspectionDto>
{
  public VehicleInspectionsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  [HttpGet]
  public override async Task<ActionResult<IEnumerable<VehicleInspectionDto>>> GetAll()
  {
    var inspections = await _context.VehicleInspections
        .Include(i => i.Incidents)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<VehicleInspectionDto>>(inspections));
  }

  [HttpGet("{id}")]
  public override async Task<ActionResult<VehicleInspectionDto>> GetById(int id)
  {
    var inspection = await _context.VehicleInspections
        .Include(i => i.Incidents)
            .ThenInclude(i => i.Photos)
        .FirstOrDefaultAsync(i => i.Id == id);

    if (inspection == null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<VehicleInspectionDto>(inspection));
  }

  // Get inspections for a specific booking
  [HttpGet("ByBooking/{bookingId}")]
  public async Task<ActionResult<IEnumerable<VehicleInspectionDto>>> GetByBooking(int bookingId)
  {
    var booking = await _context.Set<Booking>().FindAsync(bookingId);
    if (booking == null)
    {
      return NotFound("Booking not found");
    }

    var inspections = await _context.VehicleInspections
        .Include(i => i.Incidents)
        .Where(i => i.BookingId == bookingId)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<VehicleInspectionDto>>(inspections));
  }

  // Get inspections by type (pickup or return)
  [HttpGet("ByType/{bookingId}/{type}")]
  public async Task<ActionResult<VehicleInspectionDto>> GetByType(int bookingId, InspectionType type)
  {
    var inspection = await _context.VehicleInspections
        .Include(i => i.Incidents)
            .ThenInclude(i => i.Photos)
        .FirstOrDefaultAsync(i => i.BookingId == bookingId && i.Type == type);

    if (inspection == null)
    {
      return NotFound();
    }

    return Ok(_mapper.Map<VehicleInspectionDto>(inspection));
  }

  // Create inspection with custom validation
  [HttpPost]
  public override async Task<ActionResult<VehicleInspectionDto>> Create(VehicleInspectionInputDto dto)
  {
    // Verify booking exists
    var booking = await _context.Set<Booking>().FindAsync(dto.BookingId);
    if (booking == null)
    {
      return NotFound("Booking not found");
    }

    // Verify no duplicate inspection type for the same booking
    var existingInspection = await _context.VehicleInspections
        .FirstOrDefaultAsync(i => i.BookingId == dto.BookingId && i.Type == dto.Type);

    if (existingInspection != null)
    {
      return Conflict($"An inspection of type {dto.Type} already exists for this booking");
    }

    return await base.Create(dto);
  }

}