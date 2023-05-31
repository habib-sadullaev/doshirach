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

	public async Task<Item?> GetAsync(int id)
	{
		const string getItem = """
			SELECT * FROM Item WHERE id = @id
			""";
		return await dbConnection.QueryFirstOrDefaultAsync<Item>(getItem, new { id });
	}

	public async Task<Item[]> ListAsync(int categoryId, int page, int pageSize)
	{
		const string selectCategoryItemsByPage = """
			SELECT * FROM Item
			WHERE CategoryId = @categoryId
			LIMIT @limit OFFSET @offset
			""";
		var result = await dbConnection.QueryAsync<Item>(
			selectCategoryItemsByPage,
			new { categoryId, page, limit = pageSize, offset = (page - 1) * pageSize });
		return result.ToArray();
	}

	public async Task AddAsync(Item item)
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
		await dbConnection.ExecuteAsync(insertItem, item);
	}

	public async Task UpdateAsync(Item item)
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
		await dbConnection.ExecuteAsync(updateItem, item);
	}

	public async Task DeleteAsync(int id)
	{
		const string deleteItem = """
			DELETE FROM Item WHERE id = @id
			""";
		await dbConnection.ExecuteAsync(deleteItem, new { id });
	}
}