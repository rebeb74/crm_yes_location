namespace YesLocation.Domain.Entities;

public class Location : BaseModel
{
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public string PostalCode { get; set; } = string.Empty;
  public string Country { get; set; } = string.Empty;
  public bool IsActive { get; set; } = true;
  public virtual ICollection<Booking> PickupBookings { get; set; } = [];
  public virtual ICollection<Booking> ReturnBookings { get; set; } = [];
}
