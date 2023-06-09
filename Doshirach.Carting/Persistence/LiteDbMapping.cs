using Doshirach.Carting.Core.Models;
using LiteDB;

namespace Doshirach.Carting.Persistence;

public static class LiteDbMapping
{
	public static void Configure()
	{
		BsonMapper.Global.Entity<CartItem>().DbRef(x => x.Item, "items");
	}
}