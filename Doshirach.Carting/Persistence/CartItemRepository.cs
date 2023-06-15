using Doshirach.Carting.Core.Interfaces;
using Doshirach.Carting.Core.Models;
using LiteDB;

namespace Doshirach.Carting.Persistence;

public class CartItemRepository : ICartItemRepository
{
	private readonly ILiteCollection<CartItem> cartItems;
	private readonly ILiteCollection<Item> items;

	public CartItemRepository(ILiteDatabase db)
	{
		cartItems = db.GetCollection<CartItem>();
		items = db.GetCollection<Item>();
	}

	public CartItem[] GetCartItems(int cartId) => cartItems.Find(i => i.CartId == cartId).ToArray();

	public CartItem AddCartItem(int cartId, Item item, int quantity)
	{
		var cartItemKey = new BsonValue((cartId, item.Id));
		var itemKey = new BsonValue(item.Id);

		switch (cartItems.FindById(cartItemKey))
		{
			case { } existingCartItem:
				existingCartItem.Quantity += quantity;

				items.Update(itemKey, item);
				cartItems.Update(cartItemKey, existingCartItem);

				return existingCartItem;

			case null:
				var newCartItem = new CartItem
				{
					CartId = cartId, 
					Item = item, 
					Quantity = quantity
				};
				cartItems.Insert(cartItemKey, newCartItem);

				return newCartItem;
		}
	}

	public bool RemoveCartItem(int cartId, int itemId) => cartItems.Delete(new((cartId, itemId)));

	public bool UpdateItem(Item item) => items.Update(item.Id, item);
}