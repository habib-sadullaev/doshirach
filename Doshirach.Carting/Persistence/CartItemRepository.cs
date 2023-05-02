using Doshirach.Carting.Common.Interfaces;
using Doshirach.Carting.Common.Models;
using LiteDB;

namespace Doshirach.Carting.Persistence;

public class CartItemRepository : ICartItemRepository
{
	private readonly ILiteCollection<CartItem> cartItems;

	public CartItemRepository(string connectionString)
	{
		var db = new LiteDatabase(connectionString);
		cartItems = db.GetCollection<CartItem>();
	}

	public CartItem[] GetCartItems(int cartId) => cartItems.Find(i => i.CartId == cartId).ToArray();

	public void AddCartItem(CartItem cartItem)
	{
		BsonValue id(int cartId, int cartItemId) => new BsonValue((cartId, cartItemId));

		if (cartItems.FindById(id(cartItem.CartId, cartItem.Id)) is { } existingCartItem)
		{
			existingCartItem.Quantity += cartItem.Quantity;
			existingCartItem.Price = cartItem.Price;
			cartItems.Update(id(cartItem.CartId, cartItem.Id), existingCartItem);
			return;
		}

		cartItems.Insert(id(cartItem.CartId, cartItem.Id), cartItem);
	}

	public void RemoveCartItem(int cartItemId) => cartItems.Delete(cartItemId);
}