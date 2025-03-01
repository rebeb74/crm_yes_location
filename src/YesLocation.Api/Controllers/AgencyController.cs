using Microsoft.AspNetCore.Mvc;
using YesLocation.Application.DTOs.Agency;
using YesLocation.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgencyController : BaseController<Agency, AgencyInputDto, AgencyDto>
{
  public AgencyController(YesLocationDbContext context, IMapper mapper) : base(context, mapper)
  {
  }

  /// <summary>
  /// Get agency information
  /// </summary>
  /// <returns>The agency information</returns>
  [HttpGet]
  public override async Task<ActionResult<IEnumerable<AgencyDto>>> GetAll()
  {
    // Always retrieve the first (and only) record
    var agency = await _context.Set<Agency>()
        .FirstOrDefaultAsync();

    if (agency == null)
    {
      return NotFound("No agency information found");
    }

    return Ok(_mapper.Map<AgencyDto>(agency));
  }

  /// <summary>
  /// Updates the agency information
  /// </summary>
  /// <param name="id">Agency ID</param>
  /// <param name="agencyDto">Agency data to update</param>
  /// <returns>Updated agency</returns>
  [HttpPut("{id}")]
  [Authorize(Roles = "Admin")]
  public override async Task<IActionResult> Update(int id, AgencyInputDto agencyDto)
  {
    var agency = await _context.Set<Agency>().FindAsync(id);
    if (agency == null)
    {
      return NotFound($"No agency found with ID {id}");
    }

    _mapper.Map(agencyDto, agency);
    await _context.SaveChangesAsync();

    return Ok(_mapper.Map<AgencyDto>(agency));
  }

  /// <summary>
  /// Method disabled
  /// </summary>
  [HttpPost]
  public override async Task<ActionResult<AgencyDto>> Create(AgencyInputDto dto)
  {
    return await Task.FromResult(StatusCode(405));
  }

  /// <summary>
  /// Method disabled
  /// </summary>
  [HttpDelete("{id}")]
  public override async Task<IActionResult> Delete(int id)
  {
    return await Task.FromResult(StatusCode(405));
  }

  /// <summary>
  /// Method disabled
  /// </summary>
  [HttpGet("{id}")]
  public override async Task<ActionResult<AgencyDto>> GetById(int id)
  {
    return await Task.FromResult(StatusCode(405));
  }
}