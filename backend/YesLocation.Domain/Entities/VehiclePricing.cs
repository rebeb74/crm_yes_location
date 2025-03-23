namespace YesLocation.Domain.Entities;

public class VehiclePricing : BaseModel
{
  public required int VehicleId { get; set; }
  public required virtual Vehicle Vehicle { get; set; }

  public required int SeasonId { get; set; }
  public required virtual Season Season { get; set; }

  public required int DurationTierId { get; set; }
  public required virtual DurationTier DurationTier { get; set; }

  public required decimal DailyRate { get; set; }
}