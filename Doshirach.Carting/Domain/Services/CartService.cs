using Doshirach.Carting.Common.Interfaces;
using Doshirach.Carting.Common.Models;

namespace Doshirach.Carting.Domain.Services;

public class CartService
{
	private readonly ICartRepository cartRepository;
	private readonly ICartItemRepository cartItemRepository;

	public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository)
	{
		this.cartRepository = cartRepository;
		this.cartItemRepository = cartItemRepository;
	}

	public Cart CreateCart(int cartId)
	{
		if (cartId <= 0) throw new InvalidOperationException("Invalid cart id");

		return cartRepository.CreateCart(cartId);
	}

	public Cart GetCart(int cartId)
		=> cartRepository.GetCart(cartId) ?? throw new InvalidOperationException("Invalid cart Id");

	private void CheckCart(int cartId) => GetCart(cartId);

	public CartItem[] GetCartItems(int cartId)
	{
		if (cartId <= 0) throw new InvalidOperationException("Invalid cart id");

		return cartItemRepository.GetCartItems(cartId);
	}

	public void AddCartItem(int cartId, CartItem cartItem)
	{
		if (cartItem.Id <= 0)
			throw new InvalidOperationException("Invalid item id");

		if (string.IsNullOrEmpty(cartItem.Name))
			throw new InvalidOperationException("Item name cannot be empty");

		if (cartItem.Price <= 0)
			throw new InvalidOperationException("Item price must be positive");

		if (cartItem.Quantity <= 0)
			throw new InvalidOperationException("Item quantity must be positive");

		CheckCart(cartId);

		cartItemRepository.AddCartItem(cartId, cartItem);
	}

	public void RemoveCartItem(int cartId, int cartItemId)
	{
		if (cartId <= 0)
			throw new InvalidOperationException("Invalid cart id");

		if (cartItemId <= 0)
			throw new InvalidOperationException("Invalid item id");

		cartItemRepository.RemoveCartItem(cartId, cartItemId);
	}
}
