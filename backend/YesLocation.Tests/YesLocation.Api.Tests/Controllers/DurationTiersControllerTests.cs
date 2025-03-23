using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YesLocation.Domain.Entities;
using YesLocation.Tests.Common;
using YesLocation.Application.DTOs.DurationTier;
using YesLocation.Api.Controllers;
using Microsoft.EntityFrameworkCore;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class DurationTiersControllerTests : SeededContextTestBase
{
  private readonly DurationTiersController _controller;

  public DurationTiersControllerTests()
  {
    _controller = new DurationTiersController(_context, _mapper);
  }

  [Fact]
  public async Task GetAll_ReturnsAllDurationTiers()
  {
    // Arrange
    var tiers = new List<DurationTier>
    {
      new DurationTier
      {
        Name = "1-3 Days",
        MinDays = 1,
        MaxDays = 3
      },
      new DurationTier
      {
        Name = "4-7 Days",
        MinDays = 4,
        MaxDays = 7
      }
    };
    _context.AddRange(tiers);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetAll();

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<DurationTierDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var returnedTiers = Assert.IsAssignableFrom<IEnumerable<DurationTierDto>>(okResult.Value);
    Assert.Equal(tiers.Count, returnedTiers.Count());
  }

  [Fact]
  public async Task GetOrderedTiers_ReturnsTiersOrderedByMinDays()
  {
    // Arrange
    var tiers = new List<DurationTier>
    {
      new DurationTier
      {
        Name = "8-14 Days",
        MinDays = 8,
        MaxDays = 14
      },
      new DurationTier
      {
        Name = "1-3 Days",
        MinDays = 1,
        MaxDays = 3
      },
      new DurationTier
      {
        Name = "4-7 Days",
        MinDays = 4,
        MaxDays = 7
      }
    };
    _context.AddRange(tiers);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetOrderedTiers();

    // Assert
    var actionResult = Assert.IsType<ActionResult<IEnumerable<DurationTierDto>>>(result);
    var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
    var returnedTiers = Assert.IsAssignableFrom<IEnumerable<DurationTierDto>>(okResult.Value).ToList();

    // Check that tiers are ordered by MinDays
    Assert.Equal(1, returnedTiers[0].MinDays);
    Assert.Equal(4, returnedTiers[1].MinDays);
    Assert.Equal(8, returnedTiers[2].MinDays);
  }

  [Fact]
  public async Task Create_WithValidTier_ReturnsCreatedTier()
  {
    // Arrange
    var tierDto = new DurationTierInputDto
    {
      Name = "15+ Days",
      MinDays = 15,
      MaxDays = null // Open-ended tier
    };

    // Act
    var result = await _controller.Create(tierDto);

    // Assert
    var actionResult = Assert.IsType<ActionResult<DurationTierDto>>(result);
    var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
    var returnedTier = Assert.IsType<DurationTierDto>(createdAtActionResult.Value);
    Assert.Equal(tierDto.Name, returnedTier.Name);
    Assert.Equal(tierDto.MinDays, returnedTier.MinDays);
    Assert.Equal(tierDto.MaxDays, returnedTier.MaxDays);
  }

  [Fact]
  public async Task Create_WithInvalidDayRange_ReturnsBadRequest()
  {
    // Arrange
    var tierDto = new DurationTierInputDto
    {
      Name = "Invalid Range",
      MinDays = 10,
      MaxDays = 5 // MinDays > MaxDays, which is invalid
    };

    // Act
    var result = await _controller.Create(tierDto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
    Assert.Contains("MinDays must be less than or equal to MaxDays", badRequestResult?.Value?.ToString());
  }

  [Fact]
  public async Task Create_WithOverlappingRange_ReturnsBadRequest()
  {
    // Arrange
    // First, add an existing tier
    var existingTier = new DurationTier
    {
      Name = "4-7 Days",
      MinDays = 4,
      MaxDays = 7
    };
    _context.Add(existingTier);
    await _context.SaveChangesAsync();

    // Now try to create an overlapping tier
    var overlappingTierDto = new DurationTierInputDto
    {
      Name = "3-5 Days",
      MinDays = 3,
      MaxDays = 5 // Overlaps with the 4-7 days tier
    };

    // Act
    var result = await _controller.Create(overlappingTierDto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
    Assert.Contains("This range of days overlaps an existing range", badRequestResult?.Value?.ToString());
  }

  [Fact]
  public async Task Update_WithValidTier_ReturnsNoContent()
  {
    // Arrange
    // First, add an existing tier
    var existingTier = new DurationTier
    {
      Name = "4-7 Days",
      MinDays = 4,
      MaxDays = 7
    };
    _context.Add(existingTier);
    await _context.SaveChangesAsync();

    // Prepare the update DTO
    var updateDto = new DurationTierInputDto
    {
      Id = existingTier.Id,
      Name = "4-8 Days", // Changed name
      MinDays = 4,
      MaxDays = 8 // Changed MaxDays
    };

    // Act
    var result = await _controller.Update(existingTier.Id, updateDto);

    // Assert
    Assert.IsType<NoContentResult>(result);

    // Verify the update was applied
    var updatedTier = await _context.Set<DurationTier>().FindAsync(existingTier.Id);
    Assert.Equal("4-8 Days", updatedTier?.Name);
    Assert.Equal(8, updatedTier?.MaxDays);
  }

  [Fact]
  public async Task Update_WithInvalidDayRange_ReturnsBadRequest()
  {
    // Arrange
    // First, add an existing tier
    var existingTier = new DurationTier
    {
      Name = "4-7 Days",
      MinDays = 4,
      MaxDays = 7
    };
    _context.Add(existingTier);
    await _context.SaveChangesAsync();

    // Prepare the update DTO with invalid range
    var updateDto = new DurationTierInputDto
    {
      Id = existingTier.Id,
      Name = "7-4 Days", // Note the inverted range
      MinDays = 7,
      MaxDays = 4 // MinDays > MaxDays, which is invalid
    };

    // Act
    var result = await _controller.Update(existingTier.Id, updateDto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Contains("MinDays must be less than or equal to MaxDays", badRequestResult?.Value?.ToString());
  }

  [Fact]
  public async Task Update_WithOverlappingRange_ReturnsBadRequest()
  {
    // Arrange
    // Add two non-overlapping tiers
    var tier1 = new DurationTier
    {
      Name = "1-3 Days",
      MinDays = 1,
      MaxDays = 3
    };
    var tier2 = new DurationTier
    {
      Name = "4-7 Days",
      MinDays = 4,
      MaxDays = 7
    };
    _context.AddRange(tier1, tier2);
    await _context.SaveChangesAsync();

    // Try to update tier1 to overlap with tier2
    var updateDto = new DurationTierInputDto
    {
      Id = tier1.Id,
      Name = "1-5 Days", // Now overlaps with tier2
      MinDays = 1,
      MaxDays = 5 // Overlaps with the 4-7 days tier
    };

    // Act
    var result = await _controller.Update(tier1.Id, updateDto);

    // Assert
    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Contains("This range of days overlaps an existing range", badRequestResult?.Value?.ToString());
  }

  [Fact]
  public async Task Update_WithNonOverlappingRange_ReturnsNoContent()
  {
    // Arrange
    // Add two non-overlapping tiers
    var tier1 = new DurationTier
    {
      Name = "1-3 Days",
      MinDays = 1,
      MaxDays = 3
    };
    var tier2 = new DurationTier
    {
      Name = "4-7 Days",
      MinDays = 4,
      MaxDays = 7
    };
    _context.AddRange(tier1, tier2);
    await _context.SaveChangesAsync();

    // Update tier1 with a non-overlapping range
    var updateDto = new DurationTierInputDto
    {
      Id = tier1.Id,
      Name = "1-3 Days Modified",
      MinDays = 1,
      MaxDays = 3 // Same range, just a name change
    };

    // Act
    var result = await _controller.Update(tier1.Id, updateDto);

    // Assert
    Assert.IsType<NoContentResult>(result);

    // Verify the update was applied
    var updatedTier = await _context.Set<DurationTier>().FindAsync(tier1.Id);
    Assert.Equal("1-3 Days Modified", updatedTier?.Name);
  }

  [Fact]
  public async Task Delete_WithValidId_RemovesTier()
  {
    // Arrange
    var tier = new DurationTier
    {
      Name = "Test Tier",
      MinDays = 1,
      MaxDays = 3
    };
    _context.Add(tier);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.Delete(tier.Id);

    // Assert
    Assert.IsType<NoContentResult>(result);

    // Verify the tier was deleted
    var deleted = await _context.Set<DurationTier>().FindAsync(tier.Id);
    Assert.Null(deleted);
  }
}