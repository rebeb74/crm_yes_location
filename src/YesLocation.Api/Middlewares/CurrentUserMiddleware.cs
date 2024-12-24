using YesLocation.Domain.Interfaces;

namespace YesLocation.Api.Middlewares;

public class CurrentUserMiddleware(RequestDelegate next)
{
  private readonly RequestDelegate _next = next;

  public async Task Invoke(HttpContext context, ICurrentUserService currentUserService)
  {
    if (context.User.Identity?.IsAuthenticated ?? false)
    {
      var claims = context.User.Claims;
      var idClaim = claims.FirstOrDefault(c => c.Type == "userId")?.Value;
      if (int.TryParse(idClaim, out int userId))
      {
        currentUserService.Id = userId;
      }
      currentUserService.Username = claims.FirstOrDefault(c => c.Type == "username")?.Value;
      currentUserService.Email = claims.FirstOrDefault(c => c.Type == "email")?.Value;
      currentUserService.FirstName = claims.FirstOrDefault(c => c.Type == "firstName")?.Value;
      currentUserService.LastName = claims.FirstOrDefault(c => c.Type == "lastName")?.Value;
    }

    await _next(context);
  }
}