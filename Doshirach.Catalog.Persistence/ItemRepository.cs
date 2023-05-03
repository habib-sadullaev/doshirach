using System.Data;
using Dapper;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Core.Models;

namespace Doshirach.Catalog.Persistence;

public class ItemRepository : IItemRepository
{
	private readonly IDbConnection dbConnection;

	public ItemRepository(IDbConnection dbConnection)
	{
		this.dbConnection = dbConnection;
	}

	public Item? Get(int id)
	{
		const string getItem = """
			SELECT * FROM Item WHERE id = @id
			""";
		return dbConnection.QueryFirstOrDefault<Item>(getItem, new { id });
	}

	public Item[] List()
	{
		const string selectAllItems = """
			SELECT * FROM Item
			""";
		return dbConnection.Query<Item>(selectAllItems).ToArray();
	}

	public void Add(Item item)
	{
		const string insertItem = """
			INSERT INTO Item
			(
				 name
				,description
				,image
				,category_id
				,price
				,amount
			) 
			VALUES
			(
				 @Name
				,@Description
				,@Image
				,@CategoryId
				,@Price
				,@Amount
			)
			""";
		dbConnection.Execute(insertItem, item);
	}

	public void Update(Item item)
	{
		const string updateItem = """
			UPDATE Item
			SET name = @Name
				,description = @Description
				,image = @Image
				,category_id = @CategoryId
				,price = @Price
				,amount = @Amount 
			WHERE id = @Id
			""";
		dbConnection.Execute(updateItem, item);
	}

	public void Delete(int id)
	{
		const string deleteItem = """
			DELETE FROM Item WHERE id = @id
			""";
		dbConnection.Execute(deleteItem, new { id });
	}
}