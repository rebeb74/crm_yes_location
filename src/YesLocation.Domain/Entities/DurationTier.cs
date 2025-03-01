namespace YesLocation.Domain.Entities;

public class DurationTier : BaseModel
{
  public required string Name { get; set; }  // "1 jour", "2-5 jours", etc.
  public required int MinDays { get; set; }  // Durée minimale
  public int? MaxDays { get; set; }          // Durée maximale (null pour 30+)

  // Relationships
  public virtual ICollection<VehiclePricing> VehiclePricings { get; set; } = [];
}