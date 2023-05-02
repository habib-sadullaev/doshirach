using Doshirach.Carting.Common.Interfaces;
using Doshirach.Carting.Common.Models;
using LiteDB;

namespace Doshirach.Carting.Persistence;

public class CartRepository : ICartRepository
{
	private readonly LiteDatabase db;

	public CartRepository(string connectionString)
	{
		db = new(connectionString);
	}

	public CartItem[] GetCartItems(int cartId)
	{
		if (cartId <= 0) throw new ArgumentException("Invalid cart id");

		var collection = db.GetCollection<CartItem>("cartItems");

		return collection.Find(i => i.CartId == cartId).ToArray();
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

		var carts = db.GetCollection<Cart>("carts");
		var cart = carts.FindOne(c => c.Id == cartId);

		if (cart == null)
			throw new InvalidOperationException("Invalid cart Id");

		var existingItem = cart.Items.SingleOrDefault(i => i.Id == cartItem.Id);
		if (existingItem == null)
		{
			cart.Items.Add(cartItem);
		}
		else
		{
			existingItem.Quantity += cartItem.Quantity;
			existingItem.Price = cartItem.Price;
		}

		carts.Update(cart);
	}

	public void RemoveCartItem(int cartId, int cartItemId)
	{
		if (cartId <= 0)
			throw new InvalidOperationException("Invalid cart id");

		if (cartItemId <= 0)
			throw new InvalidOperationException("Invalid item id");

		var collection = db.GetCollection<CartItem>("cartItems");

		collection.Delete(cartItemId);
	}
}