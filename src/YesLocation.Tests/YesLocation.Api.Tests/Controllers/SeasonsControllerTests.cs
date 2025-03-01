using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Tests.Common;
using YesLocation.Application.DTOs.Season;
using YesLocation.Api.Controllers;
using Microsoft.EntityFrameworkCore;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class SeasonsControllerTests : SeededContextTestBase
{
  private readonly SeasonsController _controller;

  public SeasonsControllerTests()
  {
    _controller = new SeasonsController(_context, _mapper);
  }

  [Fact]
  public async Task GetSeasonsByYear_WithValidYear_ReturnsSeasons()
  {
    // Arrange
    int year = DateTime.Now.Year;

    // Let's create some seasons for this year
    var seasons = new List<Season>
    {
      new Season
      {
        Name = "Low Season",
        StartDate = new DateTime(year, 1, 1),
        EndDate = new DateTime(year, 4, 30),
        Year = year,
        Type = SeasonType.Low
      },
      new Season
      {
        Name = "Medium Season",
        StartDate = new DateTime(year, 5, 1),
        EndDate = new DateTime(year, 8, 31),
        Year = year,
        Type = SeasonType.Medium
      }
    };

    _context.AddRange(seasons);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetSeasonsByYear(year);

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<SeasonDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var returnedSeasons = Assert.IsAssignableFrom<IEnumerable<SeasonDto>>(okResult.Value);
    Assert.Equal(seasons.Count, returnedSeasons.Count());
    Assert.All(returnedSeasons, season => Assert.Equal(year, season.Year));
  }

  [Fact]
  public async Task Create_WithOverlappingSeason_ReturnsBadRequest()
  {
    // Arrange
    int year = DateTime.Now.Year;

    // Let's first create an existing season
    var existingSeason = new Season
    {
      Name = "Existing Season",
      StartDate = new DateTime(year, 6, 1),
      EndDate = new DateTime(year, 8, 31),
      Year = year,
      Type = SeasonType.Medium
    };

    _context.Add(existingSeason);
    await _context.SaveChangesAsync();

    // Now let's create an overlapping season
    var overlappingSeason = new SeasonInputDto
    {
      Name = "Overlapping Season",
      StartDate = new DateTime(year, 7, 1),
      EndDate = new DateTime(year, 9, 30),
      Year = year,
      Type = SeasonType.High
    };

    // Act
    var result = await _controller.Create(overlappingSeason);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
    Assert.Contains("straddles", badRequestResult?.Value?.ToString());
  }

  [Fact]
  public async Task GetDefaultSeason_WithValidYear_ReturnsLowSeason()
  {
    // Arrange
    int year = DateTime.Now.Year;

    // Let's create a low season for this year
    var lowSeason = new Season
    {
      Name = "Default Low Season",
      StartDate = new DateTime(year, 1, 1),
      EndDate = new DateTime(year, 12, 31),
      Year = year,
      Type = SeasonType.Low
    };

    _context.Add(lowSeason);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetDefaultSeason(year);

    // Assert
    var actionResult = Assert.IsType<ActionResult<SeasonDto>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var returnedSeason = Assert.IsType<SeasonDto>(okResult.Value);
    Assert.Equal(lowSeason.Id, returnedSeason.Id);
    Assert.Equal(SeasonType.Low, returnedSeason.Type);
  }

  [Fact]
  public async Task GetDefaultSeason_WithNoLowSeason_ReturnsNotFound()
  {
    // Arrange
    int year = DateTime.Now.Year + 1; // Next year with no defined season

    // Act
    var result = await _controller.GetDefaultSeason(year);

    // Assert
    var actionResult = Assert.IsType<ActionResult<SeasonDto>>(result);
    var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
    Assert.Contains($"No low season is defined for the year {year}", notFoundResult?.Value?.ToString());
  }

  [Fact]
  public async Task GetDefaultSeason_WithNoYearSpecified_UsesCurrentYear()
  {
    // Arrange
    int currentYear = DateTime.Now.Year;

    // Let's create a low season for the current year
    var lowSeason = new Season
    {
      Name = "Current Year Low Season",
      StartDate = new DateTime(currentYear, 1, 1),
      EndDate = new DateTime(currentYear, 12, 31),
      Year = currentYear,
      Type = SeasonType.Low
    };

    _context.Add(lowSeason);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetDefaultSeason(); // No year parameter

    // Assert
    var actionResult = Assert.IsType<ActionResult<SeasonDto>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var returnedSeason = Assert.IsType<SeasonDto>(okResult.Value);
    Assert.Equal(lowSeason.Id, returnedSeason.Id);
    Assert.Equal(currentYear, returnedSeason.Year);
  }
}