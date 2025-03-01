namespace YesLocation.Domain.Entities;

public class Location : BaseModel
{
  public string? Name { get; set; }
  public string? Address { get; set; }
  public string? City { get; set; }
  public string? PostalCode { get; set; }
  public bool IsActive { get; set; } = true;
  public virtual ICollection<Booking> PickupBookings { get; set; } = [];
  public virtual ICollection<Booking> ReturnBookings { get; set; } = [];
}
