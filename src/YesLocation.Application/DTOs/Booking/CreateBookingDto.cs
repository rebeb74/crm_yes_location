using YesLocation.Domain.Enums;
using YesLocation.Domain.Interfaces;

namespace YesLocation.Application.DTOs.Booking;

public class CreateBookingDto : IBaseDto
{
  public int? Id { get; set; }
  public int CustomerId { get; set; }
  public int VehicleId { get; set; }
  public DateTime PickupDate { get; set; }
  public DateTime ReturnDate { get; set; }
  public BookingStatus Status { get; set; }
  public string? Notes { get; set; }
  public double TotalAmount { get; set; }
}
