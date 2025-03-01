using YesLocation.Application.DTOs.Common;

namespace YesLocation.Application.DTOs.Location;

public class LocationInputDto : BaseInputModelDto
{
  public string? Name { get; set; }
  public string? Address { get; set; }
  public string? City { get; set; }
  public string? PostalCode { get; set; }
  public bool IsActive { get; set; } = true;
}
