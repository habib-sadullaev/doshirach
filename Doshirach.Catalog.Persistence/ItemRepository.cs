using Dapper;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Core.Models;
using Microsoft.Data.SqlClient;

namespace Doshirach.Catalog.Persistence;

public class ItemRepository : IItemRepository
{
	private readonly string connectionString;

	public ItemRepository(string connectionString)
	{
		this.connectionString = connectionString;
	}

	public Item Get(int id)
	{
		using var conn = new SqlConnection(connectionString);
		return conn.QueryFirstOrDefault<Item>("SELECT * FROM Item WHERE id = @id", new { id });
	}

	public List<Item> List()
	{
		using var conn = new SqlConnection(connectionString);
		return conn.Query<Item>("SELECT * FROM Item").ToList();
	}

	public void Add(Item item)
	{
		using var conn = new SqlConnection(connectionString);
		conn.Execute("INSERT INTO Item (name, description, image, category_id, price, amount) VALUES (@Name, @Description, @Image, @CategoryId, @Price, @Amount)", item);
	}

	public void Update(Item item)
	{
		using var conn = new SqlConnection(connectionString);
		conn.Execute(
			@"""UPDATE Item SET 
						 name = @Name
						,description = @Description
						,image = @Image
						,category_id = @CategoryId
						,price = @Price
						,amount = @Amount 
					WHERE id = @Id""", item);
	}

	public void Delete(int id)
	{
		using var conn = new SqlConnection(connectionString);
		conn.Execute("DELETE FROM Item WHERE id = @id", new { id });
	}
}