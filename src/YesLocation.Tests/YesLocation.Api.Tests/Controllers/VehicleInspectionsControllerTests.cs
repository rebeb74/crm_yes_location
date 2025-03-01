using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Controllers;
using YesLocation.Application.DTOs.VehicleInspection;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Tests.Common;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class VehicleInspectionsControllerTests : SeededContextTestBase
{
  private readonly VehicleInspectionsController _controller;

  public VehicleInspectionsControllerTests()
  {
    _controller = new VehicleInspectionsController(_context, _mapper);

    // Ajouter des données de test
    SetupTestData().Wait();
  }

  private async Task SetupTestData()
  {
    // Créer un booking de test s'il n'existe pas déjà
    if (!await _context.Set<Booking>().AnyAsync())
    {
      var customer = new Customer
      {
        FirstName = "Test",
        LastName = "Customer",
        Email = "test@example.com"
        // PhoneNumber n'existe pas dans l'entité Customer
      };

      var vehicle = new Vehicle
      {
        Brand = "Test Brand",
        Model = "Test Model",
        RegistrationNumber = "TEST123",
        Status = VehicleStatus.Available
      };

      _context.Add(customer);
      _context.Add(vehicle);
      await _context.SaveChangesAsync();

      var booking = new Booking
      {
        CustomerId = customer.Id,
        Customer = customer,
        VehicleId = vehicle.Id,
        Vehicle = vehicle,
        PickupDate = DateTime.Now,
        ReturnDate = DateTime.Now.AddDays(7),
        Status = BookingStatus.Confirmed
      };

      _context.Add(booking);
      await _context.SaveChangesAsync();
    }
  }

  [Fact]
  public async Task GetAll_ReturnsOkWithInspections()
  {
    // Arrange
    var booking = await _context.Set<Booking>().FirstAsync();
    var inspection = new VehicleInspection
    {
      BookingId = booking.Id,
      Booking = booking,
      Type = InspectionType.Pickup,
      InspectionDate = DateTime.Now,
      Mileage = 10000,
      VehicleCleaned = true,
      FuelLevel = FuelLevel.Full
    };

    _context.Add(inspection);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetAll();

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleInspectionDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var inspections = Assert.IsAssignableFrom<IEnumerable<VehicleInspectionDto>>(okResult.Value);
    Assert.NotEmpty(inspections);
  }

  [Fact]
  public async Task GetById_WithValidId_ReturnsInspection()
  {
    // Arrange
    var booking = await _context.Set<Booking>().FirstAsync();
    var inspection = new VehicleInspection
    {
      BookingId = booking.Id,
      Booking = booking,
      Type = InspectionType.Pickup,
      InspectionDate = DateTime.Now,
      Mileage = 10000,
      VehicleCleaned = true,
      FuelLevel = FuelLevel.Full
    };

    _context.Add(inspection);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetById(inspection.Id);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehicleInspectionDto>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var dto = Assert.IsType<VehicleInspectionDto>(okResult.Value);
    Assert.Equal(inspection.Id, dto.Id);
    Assert.Equal(inspection.Type, dto.Type);
  }

  [Fact]
  public async Task GetByBooking_WithValidId_ReturnsInspections()
  {
    // Arrange
    var booking = await _context.Set<Booking>().FirstAsync();
    var inspection = new VehicleInspection
    {
      BookingId = booking.Id,
      Booking = booking,
      Type = InspectionType.Pickup,
      InspectionDate = DateTime.Now,
      Mileage = 10000,
      VehicleCleaned = true,
      FuelLevel = FuelLevel.Full
    };

    _context.Add(inspection);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetByBooking(booking.Id);

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleInspectionDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var inspections = Assert.IsAssignableFrom<IEnumerable<VehicleInspectionDto>>(okResult.Value);
    Assert.NotEmpty(inspections);
  }

  [Fact]
  public async Task Create_WithValidData_ReturnsCreatedInspection()
  {
    // Arrange
    var booking = await _context.Set<Booking>().FirstAsync();
    var dto = new VehicleInspectionInputDto
    {
      BookingId = booking.Id,
      Type = InspectionType.Return,
      InspectionDate = DateTime.Now,
      Mileage = 10500,
      VehicleCleaned = false,
      FuelLevel = FuelLevel.Half
    };

    // Act
    var result = await _controller.Create(dto);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehicleInspectionDto>>(result);
    var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
    var createdDto = Assert.IsType<VehicleInspectionDto>(createdResult.Value);
    Assert.Equal(dto.BookingId, createdDto.BookingId);
    Assert.Equal(dto.Type, createdDto.Type);
    Assert.Equal(dto.Mileage, createdDto.Mileage);
    Assert.Equal(dto.FuelLevel, createdDto.FuelLevel);
  }

  [Fact]
  public async Task Create_WithDuplicateType_ReturnsConflict()
  {
    // Arrange
    var booking = await _context.Set<Booking>().FirstAsync();
    var inspection = new VehicleInspection
    {
      BookingId = booking.Id,
      Booking = booking,
      Type = InspectionType.Return,
      InspectionDate = DateTime.Now,
      Mileage = 10000,
      VehicleCleaned = true,
      FuelLevel = FuelLevel.Full
    };

    _context.Add(inspection);
    await _context.SaveChangesAsync();

    var dto = new VehicleInspectionInputDto
    {
      BookingId = booking.Id,
      Type = InspectionType.Return, // même type, devrait échouer
      InspectionDate = DateTime.Now,
      Mileage = 10500,
      VehicleCleaned = false,
      FuelLevel = FuelLevel.Half
    };

    // Act
    var result = await _controller.Create(dto);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehicleInspectionDto>>(result);
    Assert.IsType<ConflictObjectResult>(actionResult.Result);
  }
}