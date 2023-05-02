using Doshirach.Carting.Common.Interfaces;
using Doshirach.Carting.Common.Models;

namespace Doshirach.Carting.Domain.Services;

public class CartService
{
	private readonly ICartRepository cartRepository;

	public CartService(ICartRepository cartRepository) => this.cartRepository = cartRepository;

	public CartItem[] GetCartItems(int cartId) => cartRepository.GetCartItems(cartId);

	public void AddCartItem(int cartId, CartItem cartItem) => cartRepository.AddCartItem(cartId, cartItem);

	public void RemoveCartItem(int cartId, int cartItemId) => cartRepository.RemoveCartItem(cartId, cartItemId);
}
