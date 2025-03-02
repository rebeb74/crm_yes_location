using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.Booking;
using YesLocation.Domain.Entities;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

public class BookingsController : BaseController<Booking, BookingInputDto, BookingDto>
{
  public BookingsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  // Get Bookings by Customer Id
  [HttpGet("ByCustomer/{customerId}")]
  public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByCustomer(int customerId)
  {
    var customer = await _context.Customers.FindAsync(customerId);
    if (customer == null)
    {
      return NotFound("Customer not found");
    }

    var bookings = await _context.Bookings.Where(b => b.CustomerId == customerId).ToListAsync();
    return Ok(_mapper.Map<IEnumerable<BookingDto>>(bookings));
  }

  // Get Bookings by Vehicle Id
  [HttpGet("ByVehicle/{vehicleId}")]
  public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByVehicle(int vehicleId)
  {
    var vehicle = await _context.Vehicles.FindAsync(vehicleId);
    if (vehicle == null)
    {
      return NotFound("Vehicle not found");
    }

    var bookings = await _context.Bookings.Where(b => b.VehicleId == vehicleId).ToListAsync();
    return Ok(_mapper.Map<IEnumerable<BookingDto>>(bookings));
  }
}
