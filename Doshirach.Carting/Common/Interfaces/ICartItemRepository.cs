using Doshirach.Carting.Common.Models;

namespace Doshirach.Carting.Common.Interfaces;

public interface ICartItemRepository
{
	public CartItem[] GetCartItems(int cartId);
	public void AddCartItem(CartItem cartItem);
	public void RemoveCartItem(int cartItemId);
}
