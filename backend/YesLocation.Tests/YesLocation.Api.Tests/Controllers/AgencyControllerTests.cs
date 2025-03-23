using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using YesLocation.Api.Controllers;
using YesLocation.Application.DTOs.Agency;
using YesLocation.Domain.Entities;
using YesLocation.Tests.Common;

namespace YesLocation.Tests.YesLocation.Api.Tests.Controllers;

public class AgencyControllerTests : ContextTestBase
{
  private readonly AgencyController _controller;

  public AgencyControllerTests()
  {
    _controller = new AgencyController(_context, _mapper);
  }

  [Fact]
  public async Task GetAll_ReturnsNotFound_WhenNoAgencyExists()
  {
    // Act
    var result = await _controller.GetAll();

    // Assert
    Assert.IsType<NotFoundObjectResult>(result.Result);
  }

  [Fact]
  public async Task GetAll_ReturnsAgency_WhenAgencyExists()
  {
    // Arrange
    var agency = new Agency
    {
      Id = 1,
      CompanyName = "Test Agency",
      SiretNumber = "123456789",
      PhoneNumber = "0123456789",
      Email = "test@agency.com",
      StreetAddress = "123 Test Street",
      PostalCode = "12345",
      City = "Test City"
    };

    _context.Set<Agency>().Add(agency);
    await _context.SaveChangesAsync();

    // Act
    var result = await _controller.GetAll();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var returnedAgency = Assert.IsType<AgencyDto>(okResult.Value);
    Assert.Equal(agency.Id, returnedAgency.Id);
    Assert.Equal(agency.CompanyName, returnedAgency.CompanyName);
  }

  [Fact]
  public async Task Update_ReturnsNotFound_WhenAgencyDoesNotExist()
  {
    // Act
    var result = await _controller.Update(999, new AgencyInputDto { Id = 999 });

    // Assert
    Assert.IsType<NotFoundObjectResult>(result);
  }

  [Fact]
  public async Task Update_ReturnsUpdatedAgency_WhenAgencyExists()
  {
    // Arrange
    var agency = new Agency
    {
      Id = 1,
      CompanyName = "Test Agency",
      SiretNumber = "123456789",
      PhoneNumber = "0123456789",
      Email = "test@agency.com",
      StreetAddress = "123 Test Street",
      PostalCode = "12345",
      City = "Test City"
    };

    _context.Set<Agency>().Add(agency);
    await _context.SaveChangesAsync();

    var inputDto = new AgencyInputDto
    {
      Id = 1,
      CompanyName = "Updated Agency",
      SiretNumber = "123456789",
      PhoneNumber = "0123456789",
      Email = "test@agency.com",
      StreetAddress = "123 Test Street",
      PostalCode = "12345",
      City = "Test City"
    };

    // Act
    var result = await _controller.Update(1, inputDto);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var returnedAgency = Assert.IsType<AgencyDto>(okResult.Value);
    Assert.Equal(1, returnedAgency.Id);
    Assert.Equal("Updated Agency", returnedAgency.CompanyName);
  }

  [Fact]
  public async Task Create_ReturnsMethodNotAllowed()
  {
    // Act
    var result = await _controller.Create(new AgencyInputDto());

    // Assert
    var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
    Assert.Equal(405, statusCodeResult.StatusCode);
  }

  [Fact]
  public async Task Delete_ReturnsMethodNotAllowed()
  {
    // Act
    var result = await _controller.Delete(1);

    // Assert
    var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
    Assert.Equal(405, statusCodeResult.StatusCode);
  }

  [Fact]
  public async Task GetById_ReturnsMethodNotAllowed()
  {
    // Act
    var result = await _controller.GetById(1);

    // Assert
    var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
    Assert.Equal(405, statusCodeResult.StatusCode);
  }
}