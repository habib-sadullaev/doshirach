using Doshirach.Carting.Core.Models;

namespace Doshirach.Carting.Core.Interfaces;

public interface ICartItemRepository
{
	public CartItem[] GetCartItems(int cartId);
	public void AddCartItem(CartItem cartItem);
	public void RemoveCartItem(int cartItemId);
}
