using Doshirach.Carting.Core.Models;

namespace Doshirach.Carting.Core.Interfaces;

public interface ICartItemRepository
{
	public CartItem[] GetCartItems(int cartId);
	public bool AddCartItem(CartItem cartItem);
	public bool RemoveCartItem(int cartItemId);
}
