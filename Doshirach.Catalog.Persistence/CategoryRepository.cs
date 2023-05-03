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

	public Category? Get(int id)
	{
		const string getCategory = """
			SELECT * FROM Category WHERE id = @id
			""";
		return dbConnection.QueryFirstOrDefault<Category>(getCategory, new { id });
	}

	public Category[] List()
	{
		const string selectAllCategories = """
			SELECT * FROM Category
			""";
		return dbConnection.Query<Category>(selectAllCategories).ToArray();
	}

	public void Add(Category category)
	{
		const string insertCategory = """
			INSERT INTO Category (name, image, parent_category_id)
			VALUES (@Name, @Image, @ParentCategoryId)
			""";
		dbConnection.Execute(insertCategory, category);
	}

	public void Update(Category category)
	{
		const string updateCategory = """
			UPDATE Category
			SET name = @Name
				,image = @Image
				,parent_category_id = @ParentCategoryId
			WHERE id = @Id
			""";
		dbConnection.Execute(updateCategory, category);
	}

	public void Delete(int id)
	{
		const string deleteCategory = """
			DELETE FROM Category WHERE id = @id
			""";
		dbConnection.Execute(deleteCategory, new { id });
	}
}