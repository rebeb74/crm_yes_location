using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Controllers;
using YesLocation.Application.DTOs.VehicleIncident;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Tests.Common;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class VehicleIncidentsControllerTests : SeededContextTestBase
{
  private readonly VehicleIncidentsController _controller;
  private int _inspectionId;

  public VehicleIncidentsControllerTests()
  {
    _controller = new VehicleIncidentsController(_context, _mapper);

    // Ajouter des données de test
    SetupTestData().Wait();
  }

  private async Task SetupTestData()
  {
    // Créer un booking et une inspection de test s'ils n'existent pas déjà
    if (!await _context.Set<VehicleInspection>().AnyAsync())
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

      _inspectionId = inspection.Id;
    }
    else
    {
      _inspectionId = await _context.Set<VehicleInspection>().Select(i => i.Id).FirstAsync();
    }
  }

  [Fact]
  public async Task GetAll_ReturnsOkWithIncidents()
  {
    // Arrange
    var inspection = await _context.VehicleInspections.FindAsync(_inspectionId);
    var incident = new VehicleIncident
    {
      VehicleInspectionId = _inspectionId,
      VehicleInspection = inspection!,
      PartLocation = VehiclePartLocation.FrontLeftDoor,
      Type = IncidentType.Scratch,
      Description = "Test scratch"
    };

    _context.Add(incident);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetAll();

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleIncidentDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var incidents = Assert.IsAssignableFrom<IEnumerable<VehicleIncidentDto>>(okResult.Value);
    Assert.NotEmpty(incidents);
  }

  [Fact]
  public async Task GetById_WithValidId_ReturnsIncident()
  {
    // Arrange
    var inspection = await _context.VehicleInspections.FindAsync(_inspectionId);
    var incident = new VehicleIncident
    {
      VehicleInspectionId = _inspectionId,
      VehicleInspection = inspection!,
      PartLocation = VehiclePartLocation.FrontLeftDoor,
      Type = IncidentType.Scratch,
      Description = "Test scratch"
    };

    _context.Add(incident);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetById(incident.Id);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehicleIncidentDto>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var dto = Assert.IsType<VehicleIncidentDto>(okResult.Value);
    Assert.Equal(incident.Id, dto.Id);
    Assert.Equal(incident.PartLocation, dto.PartLocation);
    Assert.Equal(incident.Type, dto.Type);
  }

  [Fact]
  public async Task GetByInspection_WithValidId_ReturnsIncidents()
  {
    // Arrange
    var inspection = await _context.VehicleInspections.FindAsync(_inspectionId);
    var incident = new VehicleIncident
    {
      VehicleInspectionId = _inspectionId,
      VehicleInspection = inspection!,
      PartLocation = VehiclePartLocation.FrontLeftDoor,
      Type = IncidentType.Scratch,
      Description = "Test scratch"
    };

    _context.Add(incident);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetByInspection(_inspectionId);

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleIncidentDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var incidents = Assert.IsAssignableFrom<IEnumerable<VehicleIncidentDto>>(okResult.Value);
    Assert.NotEmpty(incidents);
  }

  [Fact]
  public async Task Create_WithValidData_ReturnsCreatedIncident()
  {
    // Arrange
    var dto = new VehicleIncidentInputDto
    {
      VehicleInspectionId = _inspectionId,
      PartLocation = VehiclePartLocation.RightFender,
      Type = IncidentType.Dent,
      Description = "Test dent"
    };

    // Act
    var result = await _controller.Create(dto);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehicleIncidentDto>>(result);
    var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
    var createdDto = Assert.IsType<VehicleIncidentDto>(createdResult.Value);
    Assert.Equal(dto.VehicleInspectionId, createdDto.VehicleInspectionId);
    Assert.Equal(dto.PartLocation, createdDto.PartLocation);
    Assert.Equal(dto.Type, createdDto.Type);
    Assert.Equal(dto.Description, createdDto.Description);
  }

  [Fact]
  public async Task GetByPartLocation_WithValidParams_ReturnsIncidents()
  {
    // Arrange
    var inspection = await _context.VehicleInspections.FindAsync(_inspectionId);
    var incident = new VehicleIncident
    {
      VehicleInspectionId = _inspectionId,
      VehicleInspection = inspection!,
      PartLocation = VehiclePartLocation.Dashboard,
      Type = IncidentType.Stain,
      Description = "Test stain"
    };

    _context.Add(incident);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetByPartLocation(_inspectionId, VehiclePartLocation.Dashboard);

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleIncidentDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var incidents = Assert.IsAssignableFrom<IEnumerable<VehicleIncidentDto>>(okResult.Value);
    Assert.NotEmpty(incidents);
    Assert.All(incidents, incident => Assert.Equal(VehiclePartLocation.Dashboard, incident.PartLocation));
  }
}