using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.Season;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize(Roles = "Admin")]
public class SeasonsController : BaseController<Season, SeasonInputDto, SeasonDto>
{
  public SeasonsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  // Get seasons for a specific year
  [HttpGet("ByYear/{year}")]
  public async Task<ActionResult<IEnumerable<SeasonDto>>> GetSeasonsByYear(int year)
  {
    var seasons = await _context.Set<Season>()
        .Where(s => s.Year == year)
        .OrderBy(s => s.StartDate)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<SeasonDto>>(seasons));
  }

  // Check for season overlaps when creating
  public override async Task<ActionResult<SeasonDto>> Create(SeasonInputDto dto)
  {
    if (await SeasonOverlaps(dto))
    {
      return BadRequest("The season straddles a period already defined for this year.");
    }

    return await base.Create(dto);
  }

  // Check for season overlaps when updating
  public override async Task<IActionResult> Update(int id, SeasonInputDto dto)
  {
    if (id != dto.Id)
    {
      return BadRequest();
    }

    if (await SeasonOverlaps(dto, id))
    {
      return BadRequest("The season straddles a period already defined for this year.");
    }

    return await base.Update(id, dto);
  }

  // Helper method to check for overlaps
  private async Task<bool> SeasonOverlaps(SeasonInputDto dto, int? excludeId = null)
  {
    var query = _context.Set<Season>()
        .Where(s => s.Year == dto.Year)
        .Where(s => dto.StartDate <= s.EndDate && s.StartDate <= dto.EndDate);

    if (excludeId.HasValue)
    {
      query = query.Where(s => s.Id != excludeId.Value);
    }

    return await query.AnyAsync();
  }

  // Get the default low season for a given year
  [HttpGet("DefaultSeason")]
  public async Task<ActionResult<SeasonDto>> GetDefaultSeason(int? year = null)
  {
    var targetYear = year ?? DateTime.UtcNow.Year;

    var defaultSeason = await _context.Set<Season>()
        .Where(s => s.Type == SeasonType.Low && s.Year == targetYear)
        .OrderByDescending(s => s.EndDate)
        .FirstOrDefaultAsync();

    if (defaultSeason == null)
    {
      return NotFound($"No low season is defined for the year {targetYear}");
    }

    return Ok(_mapper.Map<SeasonDto>(defaultSeason));
  }
}