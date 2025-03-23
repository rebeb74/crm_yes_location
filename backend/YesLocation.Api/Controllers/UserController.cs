using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.Role;
using YesLocation.Application.DTOs.User;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController<User, UserInputDto, UserDto>
{

  public UserController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  // PUT: api/User/5
  [HttpPut("{id}")]
  public override async Task<IActionResult> Update(int id, UserInputDto userInput)
  {
    var user = await _context.Users.FindAsync(id);
    if (user == null)

    {
      return NotFound();
    }

    var existingUserWithSameEmail = await _context.Users
        .Where(u => u.Email == userInput.Email && u.Id != id)
        .FirstOrDefaultAsync();
    if (existingUserWithSameEmail != null)
    {
      return BadRequest("A user with the same email already exists.");
    }

    var existingUserWithSameUsername = await _context.Users
        .Where(u => u.Username == userInput.Username && u.Id != id)
        .FirstOrDefaultAsync();
    if (existingUserWithSameUsername != null)
    {
      return BadRequest("A user with the same username already exists.");
    }

    return await base.Update(id, userInput);
  }

  [Authorize(Roles = "Admin")]
  [HttpPost("{userId}/roles")]
  public async Task<IActionResult> AddUserRoles(int userId, [FromBody] List<int> roleIds)
  {
    var user = await _context.Users
        .Include(u => u.UserRoles)
        .FirstOrDefaultAsync(u => u.Id == userId);

    if (user == null)
      return NotFound();

    var roles = await _context.Set<Role>()
        .Where(r => roleIds.Contains(r.Id))
        .ToListAsync();

    foreach (var role in roles)
    {
      if (!user.UserRoles.Any(ur => ur.RoleId == role.Id))
      {
        user.UserRoles.Add(new UserRole { UserId = userId, RoleId = role.Id });
      }
    }

    await _context.SaveChangesAsync();
    return Ok();
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("{userId}/roles/{roleId}")]
  public async Task<IActionResult> RemoveUserRole(int userId, int roleId)
  {
    var userRole = await _context.Set<UserRole>()
        .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

    if (userRole == null)
      return NotFound();

    _context.Set<UserRole>().Remove(userRole);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  [HttpGet("{userId}/roles")]
  public async Task<ActionResult<IEnumerable<RoleDto>>> GetUserRoles(int userId)
  {
    var roles = await _context.Set<Role>()
        .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
        .ToListAsync();

    return Ok(roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name, Value = r.Value }));
  }
}
