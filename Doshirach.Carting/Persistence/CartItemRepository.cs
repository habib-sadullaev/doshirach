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

	public void AddCartItem(int cartId, CartItem cartItem)
	{
		if (cartItems.FindById(cartItem.Id) is not { } existingCartItem)
		{
			cartItems.Insert(cartItem.Id, cartItem);
			return;
		}

		existingCartItem.Quantity += cartItem.Quantity;
		existingCartItem.Price = cartItem.Price;
		cartItems.Update(cartItem.Id, existingCartItem);
	}

	public void RemoveCartItem(int cartId, int cartItemId) => cartItems.Delete(cartItemId);
}