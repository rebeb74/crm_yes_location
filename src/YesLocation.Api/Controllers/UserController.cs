using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.Role;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController(YesLocationDbContext context) : ControllerBase
{
  private readonly YesLocationDbContext _context = context;
  IMapper _mapper = new Mapper(new MapperConfiguration(cfg =>
    {
      cfg.CreateMap<User, User>();
    }));

  // GET: api/User
  [HttpGet]
  public async Task<ActionResult<IEnumerable<User>>> GetUsers()
  {
    return await _context.Users.ToListAsync();
  }

  // GET: api/User/5
  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetUser(int id)
  {
    var user = await _context.Users.FindAsync(id);

    if (user == null)
    {
      return NotFound();
    }

    return user;
  }

  // PUT: api/User/5
  [HttpPut("{id}")]
  public async Task<ActionResult<User>> PutUser(int id, User userInput)
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

    _mapper.Map(userInput, user);
    await _context.SaveChangesAsync();

    return await GetUser(id);
  }

  // POST: api/User
  [HttpPost]
  public async Task<ActionResult<User>> PostUser(User user)
  {
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
  }

  // DELETE: api/User/5
  [Authorize(Roles = "Admin")]
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteUser(int id)
  {
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
      return NotFound();
    }

    _context.Users.Remove(user);
    await _context.SaveChangesAsync();

    return NoContent();
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

    return Ok(roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name }));
  }

  private bool UserExists(int id)
  {
    return _context.Users.Any(e => e.Id == id);
  }
}
