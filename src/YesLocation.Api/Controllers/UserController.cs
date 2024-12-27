using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
  private readonly YesLocationDbContext _context;
  IMapper _mapper;

  public UserController(YesLocationDbContext context)
  {
    _context = context;
    _mapper = new Mapper(new MapperConfiguration(cfg =>
    {
      cfg.CreateMap<User, User>();
    }));
  }

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

  private bool UserExists(int id)
  {
    return _context.Users.Any(e => e.Id == id);
  }
}
