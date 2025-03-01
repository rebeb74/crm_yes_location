using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YesLocation.Application.DTOs.VehiclePricing;
using YesLocation.Domain.Entities;
using YesLocation.Domain.Enums;
using YesLocation.Infrastructure.Persistence;

namespace YesLocation.Api.Controllers;

[Authorize(Roles = "Admin")]
public class VehiclePricingsController : BaseController<VehiclePricing, VehiclePricingInputDto, VehiclePricingDto>
{
  public VehiclePricingsController(YesLocationDbContext context, IMapper mapper)
      : base(context, mapper)
  {
  }

  // Get all the prices for a specific vehicle
  [HttpGet("ByVehicle/{vehicleId}")]
  public async Task<ActionResult<IEnumerable<VehiclePricingDto>>> GetByVehicle(int vehicleId)
  {
    var vehicle = await _context.Set<Vehicle>().FindAsync(vehicleId);
    if (vehicle == null)
    {
      return NotFound("Vehicle not found");
    }

    var pricings = await _context.Set<VehiclePricing>()
        .Include(vp => vp.Season)
        .Include(vp => vp.DurationTier)
        .Where(vp => vp.VehicleId == vehicleId)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<VehiclePricingDto>>(pricings));
  }

  // Get prices for a vehicle by season
  [HttpGet("ByVehicleAndSeason/{vehicleId}/{seasonId}")]
  public async Task<ActionResult<IEnumerable<VehiclePricingDto>>> GetByVehicleAndSeason(int vehicleId, int seasonId)
  {
    var vehicle = await _context.Set<Vehicle>().FindAsync(vehicleId);
    if (vehicle == null)
    {
      return NotFound("Vehicle not found");
    }

    var season = await _context.Set<Season>().FindAsync(seasonId);
    if (season == null)
    {
      return NotFound("Season not found");
    }

    var pricings = await _context.Set<VehiclePricing>()
        .Include(vp => vp.DurationTier)
        .Where(vp => vp.VehicleId == vehicleId && vp.SeasonId == seasonId)
        .OrderBy(vp => vp.DurationTier.MinDays)
        .ToListAsync();

    return Ok(_mapper.Map<IEnumerable<VehiclePricingDto>>(pricings));
  }

  // Override the Create method to check if the pricing already exists
  public override async Task<ActionResult<VehiclePricingDto>> Create(VehiclePricingInputDto dto)
  {
    // Check if the vehicle exists
    if (!await _context.Set<Vehicle>().AnyAsync(v => v.Id == dto.VehicleId))
    {
      return BadRequest("Vehicle not found");
    }

    // If seasonId is 0 or negative, find the most recent low season
    if (dto.SeasonId <= 0)
    {
      var currentYear = DateTime.UtcNow.Year;
      var defaultSeason = await _context.Set<Season>()
          .Where(s => s.Type == SeasonType.Low && s.Year == currentYear)
          .OrderByDescending(s => s.CreatedAt)
          .FirstOrDefaultAsync();

      if (defaultSeason != null)
      {
        dto.SeasonId = defaultSeason.Id;
      }
      else
      {
        return BadRequest("No low season is defined for the current year");
      }
    }
    // Otherwise, check if the specified season exists
    else if (!await _context.Set<Season>().AnyAsync(s => s.Id == dto.SeasonId))
    {
      return BadRequest("Season not found");
    }

    // Check if the duration tier exists
    if (!await _context.Set<DurationTier>().AnyAsync(dt => dt.Id == dto.DurationTierId))
    {
      return BadRequest("Duration tier not found");
    }

    // Check if this combination already exists
    if (await _context.Set<VehiclePricing>().AnyAsync(vp =>
        vp.VehicleId == dto.VehicleId &&
        vp.SeasonId == dto.SeasonId &&
        vp.DurationTierId == dto.DurationTierId))
    {
      return BadRequest("A price for this vehicle/season/duration combination already exists");
    }

    return await base.Create(dto);
  }

  // Calculate the price for a given period
  [HttpGet("Calculate")]
  public async Task<ActionResult<decimal>> CalculatePrice(
      int vehicleId, DateTime startDate, DateTime endDate)
  {
    var vehicle = await _context.Set<Vehicle>().FindAsync(vehicleId);
    if (vehicle == null)
    {
      return NotFound("Vehicle not found");
    }

    int totalDays = (int)(endDate - startDate).TotalDays + 1;
    if (totalDays <= 0)
    {
      return BadRequest("The end date must be after the start date");
    }

    // Find all seasons that overlap with this period
    var overlappingSeasons = await _context.Set<Season>()
        .Where(s => s.StartDate <= endDate && s.EndDate >= startDate)
        .OrderBy(s => s.StartDate)
        .ToListAsync();

    // Find the applicable duration tier
    var durationTier = await _context.Set<DurationTier>()
        .Where(dt => dt.MinDays <= totalDays &&
               (dt.MaxDays == null || dt.MaxDays >= totalDays))
        .OrderByDescending(dt => dt.MinDays)
        .FirstOrDefaultAsync();

    if (durationTier == null)
    {
      return BadRequest("No duration tier is defined for this period");
    }

    // If no season is defined at all, look for the most recent low season for the current year
    if (!overlappingSeasons.Any())
    {
      var currentYear = DateTime.UtcNow.Year;
      var defaultSeason = await _context.Set<Season>()
          .Where(s => s.Type == SeasonType.Low && s.Year == currentYear)
          .OrderByDescending(s => s.EndDate)
          .FirstOrDefaultAsync();

      if (defaultSeason != null)
      {
        overlappingSeasons = new List<Season> { defaultSeason };
      }
      else
      {
        return BadRequest("No season is defined for this period and no default low season is available");
      }
    }

    decimal totalPrice = 0;
    DateTime currentDate = startDate;

    // For each day of the rental
    while (currentDate <= endDate)
    {
      // Find the applicable season for this day
      var applicableSeason = overlappingSeasons
          .FirstOrDefault(s => currentDate >= s.StartDate && currentDate <= s.EndDate);

      // If no season is found for this day, use the default low season
      if (applicableSeason == null)
      {
        // Look for the low season for the current year
        var year = currentDate.Year;
        var defaultSeason = await _context.Set<Season>()
            .Where(s => s.Type == SeasonType.Low && s.Year == year)
            .OrderByDescending(s => s.EndDate)
            .FirstOrDefaultAsync();

        if (defaultSeason == null)
        {
          return BadRequest($"No season is defined for the date {currentDate} and no default low season is available");
        }

        applicableSeason = defaultSeason;
      }

      // Get the price for this combination
      var pricing = await _context.Set<VehiclePricing>()
          .FirstOrDefaultAsync(vp =>
              vp.VehicleId == vehicleId &&
              vp.SeasonId == applicableSeason.Id &&
              vp.DurationTierId == durationTier.Id);

      if (pricing == null)
      {
        // If no price is found for this combination, try with the low season
        if (applicableSeason.Type != SeasonType.Low)
        {
          var lowSeason = await _context.Set<Season>()
              .Where(s => s.Type == SeasonType.Low && s.Year == currentDate.Year)
              .OrderByDescending(s => s.EndDate)
              .FirstOrDefaultAsync();

          if (lowSeason != null)
          {
            var defaultPricing = await _context.Set<VehiclePricing>()
                .FirstOrDefaultAsync(vp =>
                    vp.VehicleId == vehicleId &&
                    vp.SeasonId == lowSeason.Id &&
                    vp.DurationTierId == durationTier.Id);

            if (defaultPricing != null)
            {
              totalPrice += defaultPricing.DailyRate;
              currentDate = currentDate.AddDays(1);
              continue;
            }
          }
        }

        // If still no price, error
        return BadRequest($"No pricing found for vehicle {vehicleId} on {currentDate} (season: {applicableSeason.Name}, duration tier: {durationTier.Name})");
      }
      else
      {
        totalPrice += pricing.DailyRate;
      }

      currentDate = currentDate.AddDays(1);
    }

    return Ok(totalPrice);
  }
}