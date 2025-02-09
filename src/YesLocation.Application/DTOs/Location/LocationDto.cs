namespace YesLocation.Application.DTOs.Location;

public class LocationDto
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string Phone { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
