namespace Doshirach.Catalog.Core.Models;

public class Item
{
	public required int Id { get; init; }
	public required string Name { get; init; }
	public string? Description { get; set; }
	public Image? Image { get; set; }
	public required int CategoryId { get; set; }
	public required decimal Price { get; set; }
	public required int Amount { get; set; }
}