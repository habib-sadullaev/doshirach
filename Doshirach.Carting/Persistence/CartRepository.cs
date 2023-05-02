using Doshirach.Carting.Common.Interfaces;
using Doshirach.Carting.Common.Models;
using LiteDB;

namespace Doshirach.Carting.Persistence;

public class CartRepository : ICartRepository
{
	private readonly ILiteCollection<Cart> carts;

	public CartRepository(string connectionString)
	{
		var db = new LiteDatabase(connectionString);
		carts = db.GetCollection<Cart>();
	}

	public Cart? GetCart(int cartId) => carts.FindById(cartId);

	public Cart CreateCart(int cartId)
	{
		carts.Insert(cartId, new Cart { Id = cartId });
		return carts.FindById(cartId);
	}
}
