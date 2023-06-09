using Doshirach.Carting.Core.Models;

namespace Doshirach.Carting.Core.Interfaces;

public interface ICartItemRepository
{
	public CartItem[] GetCartItems(int cartId);
	public CartItem AddCartItem(int cartId, Item item, int quantity);
	public bool RemoveCartItem(int cartId, int itemId);
	public bool UpdateItem(Item item);
}
