namespace Doshirach.Carting.Common.Models;

public class Cart
{
	public required int Id { get; init; }
	public ICollection<CartItem> Items { get; init; } = new List<CartItem>();
}