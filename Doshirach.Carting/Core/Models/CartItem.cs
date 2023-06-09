namespace Doshirach.Carting.Core.Models;

public record CartItem
{
	public required int CartId { get; init; }
	public required Item Item { get; init; }
	public required int Quantity { get; set; }
}