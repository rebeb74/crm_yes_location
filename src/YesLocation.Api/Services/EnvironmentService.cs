using YesLocation.Domain.Interfaces;

namespace YesLocation.Api.Services;

public class EnvironmentService(IWebHostEnvironment webHostEnvironment) : IEnvironmentService
{
  private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

  public bool IsDevelopment() => _webHostEnvironment.IsDevelopment();
}