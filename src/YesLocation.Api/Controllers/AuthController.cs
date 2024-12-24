using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Services;
using YesLocation.Application.DTOs;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Interfaces;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize]
[Route("[controller]")]
public class AuthController : ControllerBase
{

  private readonly YesLocationDbContext _ctx;
  private readonly AuthService _authService;

  public AuthController(YesLocationDbContext context, IConfiguration configuration, IEnvironmentService env)
  {
    _ctx = context;
    _authService = new AuthService(configuration);
  }

  [AllowAnonymous]
  [HttpPost("Register")]
  public async Task<IActionResult> Register(UserRegistrationDto user)
  {
    if (!ModelState.IsValid)
      return BadRequest(ModelState);

    int existingUser = await _ctx!.Auth
      .Where(q => q.Email == user.Email || q.Username == user.Username)
      .CountAsync();

    if (existingUser > 0) return BadRequest("A user already exists with this email address or username.");

    byte[] passwordSalt = [128 / 8];
    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    {
      rng.GetNonZeroBytes(passwordSalt);
    }

    byte[] passwordHash = _authService!.GetPasswordHash(user.Password, passwordSalt);

    Auth auth = new()
    {
      Email = user.Email,
      Username = user.Username,
      PasswordHash = passwordHash,
      PasswordSalt = passwordSalt
    };

    await _ctx.Auth.AddAsync(auth);
    bool authUserAdded = await _ctx.SaveChangesAsync() > 0;

    if (!authUserAdded) return BadRequest("The user could not be registered.");

    User newUser = new()
    {
      Email = user.Email,
      Username = user.Username,
      FirstName = user.FirstName,
      LastName = user.LastName
    };
    await _ctx.Users.AddAsync(newUser);
    bool userAdded = await _ctx.SaveChangesAsync() > 0;

    if (!userAdded) return BadRequest("The user could not be registered.");

    return Ok();
  }

  [AllowAnonymous]
  [HttpPost("Login")]
  public async Task<IActionResult> Login(UserLoginDto userLoginDto)
  {
    Auth? auth = await _ctx!.Auth.FirstOrDefaultAsync(q => q.Username == userLoginDto.UsernameOrEmail || q.Email == userLoginDto.UsernameOrEmail);

    if (auth == null) return Unauthorized("Invalid username or email");

    if (auth.PasswordHash == null || auth.PasswordSalt == null) return Unauthorized("Invalid password");

    byte[] passwordHash = _authService!.GetPasswordHash(userLoginDto.Password, auth.PasswordSalt);

    if (!_authService.VerifyPassword(auth.PasswordHash, passwordHash))
      return Unauthorized("Invalid password");

    User? user = await _ctx.Users.FirstOrDefaultAsync(q => q.Username == userLoginDto.UsernameOrEmail || q.Email == userLoginDto.UsernameOrEmail);

    if (user == null) return Unauthorized("Invalid username or email");

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