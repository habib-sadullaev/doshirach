namespace Doshirach.Catalog.Core.Models;

public class Category
{
	public required int Id { get; init; }
	public required string Name { get; init; }
	public string? Image { get; set; }
	public int? ParentCategoryId { get; set; }
}
