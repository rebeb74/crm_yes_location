using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Api.Controllers;
using YesLocation.Application.DTOs.VehicleIncidentPhoto;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Tests.Common;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class VehicleIncidentPhotosControllerTests : SeededContextTestBase
{
  private readonly VehicleIncidentPhotosController _controller;
  private int _incidentId;

  public VehicleIncidentPhotosControllerTests()
  {
    _controller = new VehicleIncidentPhotosController(_context, _mapper);

    // Ajouter des données de test
    SetupTestData().Wait();
  }

  private async Task SetupTestData()
  {
    // Créer un booking, une inspection et un incident de test s'ils n'existent pas déjà
    if (!await _context.Set<VehicleIncident>().AnyAsync())
    {
      var customer = new Customer
      {
        FirstName = "Test",
        LastName = "Customer",
        Email = "test@example.com"
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

      var incident = new VehicleIncident
      {
        VehicleInspectionId = inspection.Id,
        VehicleInspection = inspection,
        PartLocation = VehiclePartLocation.FrontLeftDoor,
        Type = IncidentType.Scratch,
        Description = "Test scratch"
      };

      _context.Add(incident);
      await _context.SaveChangesAsync();

      _incidentId = incident.Id;
    }
    else
    {
      _incidentId = await _context.Set<VehicleIncident>().Select(i => i.Id).FirstAsync();
    }
  }

  [Fact]
  public async Task GetAll_ReturnsOkWithPhotos()
  {
    // Arrange
    var incident = await _context.VehicleIncidents.FindAsync(_incidentId);
    var photo = new VehicleIncidentPhoto
    {
      VehicleIncidentId = _incidentId,
      VehicleIncident = incident!,
      PhotoUrl = "https://example.com/photo1.jpg",
      Caption = "Test photo"
    };

    _context.Add(photo);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetAll();

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleIncidentPhotoDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var photos = Assert.IsAssignableFrom<IEnumerable<VehicleIncidentPhotoDto>>(okResult.Value);
    Assert.NotEmpty(photos);
  }

  [Fact]
  public async Task GetById_WithValidId_ReturnsPhoto()
  {
    // Arrange
    var incident = await _context.VehicleIncidents.FindAsync(_incidentId);
    var photo = new VehicleIncidentPhoto
    {
      VehicleIncidentId = _incidentId,
      VehicleIncident = incident!,
      PhotoUrl = "https://example.com/photo2.jpg",
      Caption = "Test photo 2"
    };

    _context.Add(photo);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetById(photo.Id);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehicleIncidentPhotoDto>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var dto = Assert.IsType<VehicleIncidentPhotoDto>(okResult.Value);
    Assert.Equal(photo.Id, dto.Id);
    Assert.Equal(photo.PhotoUrl, dto.PhotoUrl);
    Assert.Equal(photo.Caption, dto.Caption);
  }

  [Fact]
  public async Task GetByIncident_WithValidId_ReturnsPhotos()
  {
    // Arrange
    var incident = await _context.VehicleIncidents.FindAsync(_incidentId);
    var photo = new VehicleIncidentPhoto
    {
      VehicleIncidentId = _incidentId,
      VehicleIncident = incident!,
      PhotoUrl = "https://example.com/photo3.jpg",
      Caption = "Test photo 3"
    };

    _context.Add(photo);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetByIncident(_incidentId);

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehicleIncidentPhotoDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var photos = Assert.IsAssignableFrom<IEnumerable<VehicleIncidentPhotoDto>>(okResult.Value);
    Assert.NotEmpty(photos);
    Assert.All(photos, photo => Assert.Equal(_incidentId, photo.VehicleIncidentId));
  }

  [Fact]
  public async Task Create_WithValidData_ReturnsCreatedPhoto()
  {
    // Arrange
    var dto = new VehicleIncidentPhotoInputDto
    {
      VehicleIncidentId = _incidentId,
      PhotoUrl = "https://example.com/photo4.jpg",
      Caption = "Test photo 4"
    };

    // Act
    var result = await _controller.Create(dto);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehicleIncidentPhotoDto>>(result);
    var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
    var createdDto = Assert.IsType<VehicleIncidentPhotoDto>(createdResult.Value);
    Assert.Equal(dto.VehicleIncidentId, createdDto.VehicleIncidentId);
    Assert.Equal(dto.PhotoUrl, createdDto.PhotoUrl);
    Assert.Equal(dto.Caption, createdDto.Caption);
  }

  [Fact]
  public async Task Create_WithInvalidIncidentId_ReturnsNotFound()
  {
    // Arrange
    var dto = new VehicleIncidentPhotoInputDto
    {
      VehicleIncidentId = -1, // ID invalide
      PhotoUrl = "https://example.com/photo5.jpg",
      Caption = "Test photo 5"
    };

    // Act
    var result = await _controller.Create(dto);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehicleIncidentPhotoDto>>(result);
    Assert.IsType<NotFoundObjectResult>(actionResult.Result);
  }
}