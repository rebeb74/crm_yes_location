using YesLocation.Domain.Enums;

namespace YesLocation.Application.DTOs.Booking;

public class BookingDto
{
  public int Id { get; set; }
  public int CustomerId { get; set; }
  public int VehicleId { get; set; }
  public DateTime PickupDate { get; set; }
  public DateTime ReturnDate { get; set; }
  public BookingStatus Status { get; set; }
  public double TotalAmount { get; set; }
  public string? Notes { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
}
