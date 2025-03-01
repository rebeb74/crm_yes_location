namespace YesLocation.Domain.Interfaces;

public interface IBaseInputModelDto
{
  int? Id { get; set; }
  DateTime? CreatedAt { get; set; }
  DateTime? UpdatedAt { get; set; }
  int? CreatedBy { get; set; }
  int? UpdatedBy { get; set; }
}
