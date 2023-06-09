using Doshirach.Carting.Core.Interfaces;
using Doshirach.Carting.Core.Models;

namespace Doshirach.Carting.Domain.Services;

public class CartService
{
	private readonly ICartItemRepository cartItemRepository;

	public CartService(ICartItemRepository cartItemRepository)
	{
		this.cartItemRepository = cartItemRepository;
	}

	public CartItem[] GetCartItems(int cartId)
	{
		if (cartId <= 0) throw new InvalidOperationException("Invalid cart id");

		return cartItemRepository.GetCartItems(cartId);
	}

	public CartItem AddCartItem(int cartId, Item item, int quantity)
	{
		if (item.Id <= 0)
			throw new InvalidOperationException("Invalid item id");

		if (string.IsNullOrEmpty(item.Name))
			throw new InvalidOperationException("Item name cannot be empty");

		if (item.Price <= 0)
			throw new InvalidOperationException("Item price must be positive");

		if (quantity <= 0)
			throw new InvalidOperationException("Item quantity must be positive");

		return cartItemRepository.AddCartItem(cartId, item, quantity);
	}

	public bool RemoveCartItem(int cartId, int itemId)
	{
		if (cartId <= 0)
			throw new InvalidOperationException("Invalid cart id");

		if (itemId <= 0)
			throw new InvalidOperationException("Invalid item id");

		return cartItemRepository.RemoveCartItem(cartId, itemId);
	}

	public void UpdateItem(Item item)
	{
		if (item.Id <= 0)
			throw new InvalidOperationException("Invalid item id");

		if (string.IsNullOrEmpty(item.Name))
			throw new InvalidOperationException("Item name cannot be empty");

		if (item.Price <= 0)
			throw new InvalidOperationException("Item price must be positive");

		cartItemRepository.UpdateItem(item);
	}
}
