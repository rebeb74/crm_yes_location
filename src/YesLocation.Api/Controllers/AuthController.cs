using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Services;
using YesLocation.Application.DTOs.User;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
[Route("[controller]")]
public class AuthController(YesLocationDbContext context, IConfiguration configuration) : ControllerBase
{

  private readonly YesLocationDbContext _ctx = context;
  private readonly AuthService _authService = new AuthService(configuration);

  [AllowAnonymous]
  [HttpPost("Register")]
  public async Task<IActionResult> Register(UserRegistrationDto userDto)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    // Vérifier si l'utilisateur existe déjà
    bool userExists = await _ctx.Users.AnyAsync(u =>
        u.Email == userDto.Email ||
        u.Username == userDto.Username);

    if (userExists)
      return BadRequest("A user already exists with this email address or username.");

    // Créer l'utilisateur
    User newUser = new()
    {
      Email = userDto.Email,
      Username = userDto.Username,
      FirstName = userDto.FirstName,
      LastName = userDto.LastName
    };
    await _ctx.Users.AddAsync(newUser);
    await _ctx.SaveChangesAsync();

    // Créer l'authentification
    byte[] passwordSalt = _authService!.GetPasswordSalt();
    byte[] passwordHash = _authService!.GetPasswordHash(userDto.Password, passwordSalt);

    Auth auth = new()
    {
      UserId = newUser.Id,
      PasswordHash = passwordHash,
      PasswordSalt = passwordSalt
    };

    await _ctx.Auth.AddAsync(auth);
    bool authAdded = await _ctx.SaveChangesAsync() > 0;

    if (!authAdded)
    {
      _ctx.Users.Remove(newUser);
      await _ctx.SaveChangesAsync();
      return BadRequest("The user could not be registered.");
    }

    return Ok();
  }

  [AllowAnonymous]
  [HttpPost("Login")]
  public async Task<IActionResult> Login(UserLoginDto userLoginDto)
  {
    var user = await _ctx.Users
        .Include(u => u.UserRoles)
        .ThenInclude(ur => ur.Role)
        .FirstOrDefaultAsync(u =>
            u.Username == userLoginDto.UsernameOrEmail ||
            u.Email == userLoginDto.UsernameOrEmail);

    if (user == null)
      return Unauthorized("Invalid credentials");

    var auth = await _ctx.Auth
        .FirstOrDefaultAsync(a => a.UserId == user.Id);

    if (auth?.PasswordHash == null || auth.PasswordSalt == null)
      return Unauthorized("Invalid credentials");

    byte[] passwordHash = _authService!.GetPasswordHash(userLoginDto.Password, auth.PasswordSalt);

    if (!_authService.VerifyPassword(auth.PasswordHash, passwordHash))
      return Unauthorized("Invalid credentials");

    UserLogged userLogged = new()
    {
      Username = user.Username,
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      Token = _authService.CreateToken(user)
    };

    return Ok(userLogged);
  }

  [HttpGet("RefreshToken")]
  public string RefreshToken()
  {
    Claim? claim = User.FindFirst("userId") ?? throw new UnauthorizedAccessException("Unable to refresh the token.");

    int userId = int.Parse(claim.Value);

    User? user = _ctx.Users.Find(userId) ?? throw new UnauthorizedAccessException("Unable to refresh the token.");

    return _authService!.CreateToken(user);
  }
}