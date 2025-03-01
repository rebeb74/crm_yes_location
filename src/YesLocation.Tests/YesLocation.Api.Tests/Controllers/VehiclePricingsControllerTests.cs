using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Tests.Common;
using YesLocation.Application.DTOs.VehiclePricing;
using YesLocation.Api.Controllers;
using Microsoft.EntityFrameworkCore;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class VehiclePricingsControllerTests : SeededContextTestBase
{
  private readonly VehiclePricingsController _controller;

  public VehiclePricingsControllerTests()
  {
    _controller = new VehiclePricingsController(_context, _mapper);
  }

  [Fact]
  public async Task GetByVehicle_WithValidId_ReturnsPricings()
  {
    // Arrange
    // Create a vehicle with ID 1
    var vehicle = new Vehicle
    {
      Id = 1,
      Brand = "Toyota",
      Model = "Corolla"
    };
    _context.Add(vehicle);
    await _context.SaveChangesAsync();

    int vehicleId = 1; // Now a vehicle with this ID exists in the test context

    // Act
    var result = await _controller.GetByVehicle(vehicleId);

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehiclePricingDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var pricings = Assert.IsAssignableFrom<IEnumerable<VehiclePricingDto>>(okResult.Value);
    // Since we don't have any pricings yet, we just check that it returns an empty list, not null
    Assert.NotNull(pricings);
  }

  [Fact]
  public async Task GetByVehicle_WithInvalidId_ReturnsNotFound()
  {
    // Arrange
    int nonExistentVehicleId = 999;

    // Act
    var result = await _controller.GetByVehicle(nonExistentVehicleId);

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<VehiclePricingDto>>>(result);
    Assert.IsType<NotFoundObjectResult>(actionResult.Result);
  }

  [Fact]
  public async Task Create_WithDefaultLowSeason_AssignsCorrectSeasonId()
  {
    // Arrange
    // Create a vehicle with ID 1
    var vehicle = new Vehicle
    {
      Id = 1,
      Brand = "Toyota",
      Model = "Corolla"
    };
    _context.Add(vehicle);

    // Create a duration tier with ID 1
    var durationTier = new DurationTier
    {
      Id = 1,
      Name = "1-3 Days",
      MinDays = 1,
      MaxDays = 3
    };
    _context.Add(durationTier);

    // Let's add a low season to the context
    var lowSeason = new Season
    {
      Name = "Low Season Test",
      StartDate = new DateTime(DateTime.Now.Year, 1, 1),
      EndDate = new DateTime(DateTime.Now.Year, 4, 30),
      Year = DateTime.Now.Year,
      Type = SeasonType.Low
    };
    _context.Add(lowSeason);
    await _context.SaveChangesAsync();

    var vehicleId = 1; // Now a vehicle with this ID exists
    var durationTierId = 1; // Now a duration tier with this ID exists

    var pricingDto = new VehiclePricingInputDto
    {
      VehicleId = vehicleId,
      SeasonId = 0, // Request to use the default low season
      DurationTierId = durationTierId,
      DailyRate = 50.0m
    };

    // Act
    var result = await _controller.Create(pricingDto);

    // Assert
    var actionResult = Assert.IsType<ActionResult<VehiclePricingDto>>(result);
    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
    var returnedPricing = Assert.IsType<VehiclePricingDto>(createdAtActionResult.Value);

    // Verify that the assigned season is the one we created
    Assert.Equal(lowSeason.Id, returnedPricing.SeasonId);
  }

  [Fact]
  public async Task CalculatePrice_WithValidParameters_ReturnsCorrectPrice()
  {
    // Arrange
    // Create a vehicle with ID 1
    var vehicle = new Vehicle
    {
      Id = 1,
      Brand = "Toyota",
      Model = "Corolla"
    };
    _context.Add(vehicle);

    int vehicleId = vehicle.Id;
    var startDate = DateTime.Today;
    var endDate = DateTime.Today.AddDays(5);

    // Let's create a season that covers this period
    var season = new Season
    {
      Name = "Test Season",
      StartDate = startDate.AddDays(-10),
      EndDate = endDate.AddDays(10),
      Year = startDate.Year,
      Type = SeasonType.Medium
    };
    _context.Add(season);

    // Let's create a duration tier that covers 5 days
    var durationTier = new DurationTier
    {
      Name = "5-7 Days",
      MinDays = 5,
      MaxDays = 7
    };
    _context.Add(durationTier);

    // Let's create a pricing for this combination
    var pricing = new VehiclePricing
    {
      VehicleId = vehicleId,
      SeasonId = season.Id,
      DurationTierId = durationTier.Id,
      DailyRate = 100.0m,
      Vehicle = vehicle,
      Season = season,
      DurationTier = durationTier
    };
    _context.Add(pricing);

    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.CalculatePrice(vehicleId, startDate, endDate);

    // Assert
    var actionResult = Assert.IsType<ActionResult<decimal>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var totalPrice = Assert.IsType<decimal>(okResult.Value);

    // 6 days (inclusive) at 100€ per day = 600€
    Assert.Equal(600m, totalPrice);
  }

  [Fact]
  public async Task CalculatePrice_WithLowSeasonFallback_UsesLowSeasonPricing()
  {
    // Arrange
    // Create a vehicle with ID 1
    var vehicle = new Vehicle
    {
      Id = 1,
      Brand = "Toyota",
      Model = "Corolla"
    };
    _context.Add(vehicle);

    int vehicleId = vehicle.Id;
    var startDate = DateTime.Today;
    var endDate = DateTime.Today.AddDays(2);

    // Let's create a low season that will serve as a fallback
    var lowSeason = new Season
    {
      Name = "Low Season",
      StartDate = new DateTime(startDate.Year, 1, 1),
      EndDate = new DateTime(startDate.Year, 12, 31),
      Year = startDate.Year,
      Type = SeasonType.Low
    };
    _context.Add(lowSeason);

    // Let's create a duration tier
    var durationTier = new DurationTier
    {
      Name = "1-4 Days",
      MinDays = 1,
      MaxDays = 4
    };
    _context.Add(durationTier);

    // Let's create a pricing for the low season
    var pricing = new VehiclePricing
    {
      VehicleId = vehicleId,
      SeasonId = lowSeason.Id,
      DurationTierId = durationTier.Id,
      DailyRate = 50.0m,
      Vehicle = vehicle,
      Season = lowSeason,
      DurationTier = durationTier
    };
    _context.Add(pricing);

    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.CalculatePrice(vehicleId, startDate, endDate);

    // Assert
    var actionResult = Assert.IsType<ActionResult<decimal>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var totalPrice = Assert.IsType<decimal>(okResult.Value);

    // 3 days (inclusive) at 50€ per day = 150€
    Assert.Equal(150m, totalPrice);
  }
}