using System.ComponentModel.DataAnnotations.Schema;

namespace YesLocation.Domain.Entities;

[NotMapped]
public abstract class BaseModel
{
  public int Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public int? CreatedBy { get; set; }
  public int? UpdatedBy { get; set; }

}