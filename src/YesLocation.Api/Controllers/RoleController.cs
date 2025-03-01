using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.Role;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class RoleController(YesLocationDbContext context) : ControllerBase
{
  private readonly YesLocationDbContext _context = context;

  // GET: api/Role
  [HttpGet]
  public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
  {
    var roles = await _context.Roles.ToListAsync();
    return Ok(roles.Select(r => new RoleDto
    {
      Id = r.Id,
      Name = r.Name,
      Value = r.Value,
      Description = r.Description
    }));
  }

  // GET: api/Role/5
  [HttpGet("{id}")]
  public async Task<ActionResult<RoleDto>> GetRole(int id)
  {
    var role = await _context.Roles.FindAsync(id);

    if (role == null)
    {
      return NotFound();
    }

    return new RoleDto
    {
      Id = role.Id,
      Name = role.Name,
      Value = role.Value,
      Description = role.Description
    };
  }

  // PUT: api/Role/5
  [HttpPut("{id}")]
  public async Task<IActionResult> PutRole(int id, RoleDto roleDto)
  {
    var role = await _context.Roles.FindAsync(id);

    if (role == null)
    {
      return NotFound();
    }

    // Vérifier si le nom est déjà utilisé par un autre rôle
    if (await _context.Roles.AnyAsync(r => r.Name == roleDto.Name && r.Id != id))
    {
      return BadRequest($"The '{roleDto.Name}' role already exists.");
    }

    role.Name = roleDto.Name;
    role.Value = roleDto.Value;
    role.Description = roleDto.Description;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!RoleExists(id))
      {
        return NotFound();
      }
      throw;
    }

    return NoContent();
  }

  // POST: api/Role
  [HttpPost]
  public async Task<ActionResult<RoleDto>> PostRole(RoleDto roleDto)
  {
    // Vérifier si le nom est déjà utilisé
    if (await _context.Roles.AnyAsync(r => r.Name == roleDto.Name))
    {
      return BadRequest($"The '{roleDto.Name}' role already exists.");
    }

    var role = new Role
    {
      Name = roleDto.Name,
      Value = roleDto.Value,
      Description = roleDto.Description
    };

    _context.Roles.Add(role);
    await _context.SaveChangesAsync();

    roleDto.Id = role.Id;
    return CreatedAtAction(nameof(GetRole), new { id = role.Id }, roleDto);
  }

  // DELETE: api/Role/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteRole(int id)
  {
    var role = await _context.Roles
        .Include(r => r.UserRoles)
        .FirstOrDefaultAsync(r => r.Id == id);

    if (role == null)
    {
      return NotFound();
    }

    // Vérifier si des utilisateurs sont associés à ce rôle
    if (role.UserRoles.Any())
    {
      return BadRequest("Unable to delete a role associated with users.");
    }

    _context.Roles.Remove(role);
    await _context.SaveChangesAsync();

    return NoContent();
  }

  private bool RoleExists(int id)
  {
    return _context.Roles.Any(e => e.Id == id);
  }
}