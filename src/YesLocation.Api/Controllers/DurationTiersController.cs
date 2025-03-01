using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.DurationTier;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize(Roles = "Admin")]
public class DurationTiersController : BaseController<DurationTier, DurationTierInputDto, DurationTierDto>
{
  public DurationTiersController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  // Additional validation for creation
  public override async Task<ActionResult<DurationTierDto>> Create(DurationTierInputDto dto)
  {
    // Validation of day ranges
    if (dto.MaxDays.HasValue && dto.MinDays > dto.MaxDays.Value)
    {
      return BadRequest("MinDays must be less than or equal to MaxDays");
    }

    // Check for overlaps
    if (await RangeOverlaps(dto))
    {
      return BadRequest("This range of days overlaps an existing range");
    }

    return await base.Create(dto);
  }

  // Additional validation for the update
  public override async Task<IActionResult> Update(int id, DurationTierInputDto dto)
  {
    if (id != dto.Id)
    {
      return BadRequest();
    }

    // Validation of day ranges
    if (dto.MaxDays.HasValue && dto.MinDays > dto.MaxDays.Value)
    {
      return BadRequest("MinDays must be less than or equal to MaxDays");
    }

    // Check for overlaps
    if (await RangeOverlaps(dto, id))
    {
      return BadRequest("This range of days overlaps an existing range");
    }

    return await base.Update(id, dto);
  }

  // Obtain bearings sorted by duration
  [HttpGet("Ordered")]
  public async Task<ActionResult<IEnumerable<DurationTierDto>>> GetOrderedTiers()
  {
    var tiers = await _context.Set<DurationTier>()
        .OrderBy(dt => dt.MinDays)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<DurationTierDto>>(tiers));
  }

  // Helper method for checking overlaps
  private async Task<bool> RangeOverlaps(DurationTierInputDto dto, int? excludeId = null)
  {
    var query = _context.Set<DurationTier>().AsQueryable();

    if (excludeId.HasValue)
    {
      query = query.Where(dt => dt.Id != excludeId.Value);
    }

    // Logic for detecting overlaps
    // - Case where MaxDays is null (open at the top)
    // - Cases where ranges overlap
    return await query.AnyAsync(dt =>
        (dto.MaxDays == null && dt.MinDays >= dto.MinDays) ||
        (dt.MaxDays == null && dto.MinDays <= dt.MinDays) ||
        (dto.MaxDays != null && dt.MaxDays != null &&
         dto.MinDays <= dt.MaxDays && dt.MinDays <= dto.MaxDays));
  }
}