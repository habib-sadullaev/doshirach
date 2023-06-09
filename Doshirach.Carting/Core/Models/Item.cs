namespace Doshirach.Carting.Core.Models;

public record Item
{
	public required int Id { get; init; }
	public required string Name { get; init; }
	public required Image Image { get; set; }
	public required decimal Price { get; set; }
	public required int Amount { get; set;}
}