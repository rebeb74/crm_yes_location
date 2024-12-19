using YesLocation.Domain.Interfaces;

namespace YesLocation.Api.Services;

public class EnvironmentService : IEnvironmentService
{
  private readonly IWebHostEnvironment _webHostEnvironment;

  public EnvironmentService(IWebHostEnvironment webHostEnvironment)
  {
    _webHostEnvironment = webHostEnvironment;
  }

  public bool IsDevelopment() => _webHostEnvironment.IsDevelopment();
}