namespace Doshirach.Carting.Core.Models;

public class CartItem
{
	public required int Id { get; init; }
	public required int CartId { get; init; }
	public required string Name { get; init; }
	public required Image Image { get; set; }
	public required decimal Price { get; set; }
	public required int Quantity { get; set; }
}