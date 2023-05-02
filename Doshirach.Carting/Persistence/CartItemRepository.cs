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
		var id = new BsonValue((cartItem.CartId, cartItem.Id));

		switch (cartItems.FindById(id))
		{
			case { } existingCartItem:
				existingCartItem.Quantity += cartItem.Quantity;
				existingCartItem.Price = cartItem.Price;
				cartItems.Update(id, existingCartItem);
				break;
			case null:
				cartItems.Insert(id, cartItem);
				break;
		}
	}

	public void RemoveCartItem(int cartItemId) => cartItems.Delete(cartItemId);
}