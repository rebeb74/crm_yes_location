using YesLocation.Domain.Enums;

namespace YesLocation.Domain.Entities;

public class Season : BaseModel
{
  public required string Name { get; set; }  // "Basse", "Moyenne", "Haute"
  public required DateTime StartDate { get; set; }
  public required DateTime EndDate { get; set; }
  public required int Year { get; set; }  // Pour gérer les saisons annuelles
  public required SeasonType Type { get; set; }  // Enum: Low, Medium, High

  // Relationships
  public virtual ICollection<VehiclePricing> VehiclePricings { get; set; } = [];
}