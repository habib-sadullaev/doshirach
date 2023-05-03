using System.Data;
using Dapper;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Core.Models;

namespace Doshirach.Catalog.Persistence;

public class CategoryRepository : ICategoryRepository
{
	private readonly IDbConnection dbConnection;

	public CategoryRepository(IDbConnection dbConnection)
	{
		this.dbConnection = dbConnection;
	}

	public async Task<Category?> GetAsync(int id)
	{
		const string getCategory = """
			SELECT * FROM Category WHERE id = @id
			""";
		return await dbConnection.QueryFirstOrDefaultAsync<Category>(getCategory, new { id });
	}

	public async Task<Category[]> ListAsync()
	{
		const string selectAllCategories = """
			SELECT * FROM Category
			""";
		var result = await dbConnection.QueryAsync<Category>(selectAllCategories);
		return result.ToArray();
	}

	public async Task AddAsync(Category category)
	{
		const string insertCategory = """
			INSERT INTO Category (name, image, parent_category_id)
			VALUES (@Name, @Image, @ParentCategoryId)
			""";
		await dbConnection.ExecuteAsync(insertCategory, category);
	}

	public async Task UpdateAsync(Category category)
	{
		const string updateCategory = """
			UPDATE Category
			SET name = @Name
				,image = @Image
				,parent_category_id = @ParentCategoryId
			WHERE id = @Id
			""";
		await dbConnection.ExecuteAsync(updateCategory, category);
	}

	public async Task DeleteAsync(int id)
	{
		const string deleteCategory = """
			DELETE FROM Category WHERE id = @id
			""";
		await dbConnection.ExecuteAsync(deleteCategory, new { id });
	}
}