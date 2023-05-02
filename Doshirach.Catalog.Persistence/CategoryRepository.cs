using Dapper;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Core.Models;
using Microsoft.Data.SqlClient;

namespace Doshirach.Catalog.Persistence;

public class CategoryRepository : ICategoryRepository
{
	private readonly string connectionString;

	public CategoryRepository(string connectionString)
	{
		this.connectionString = connectionString;
	}

	public Category Get(int id)
	{
		using var conn = new SqlConnection(connectionString);
		return conn.QueryFirstOrDefault<Category>("SELECT * FROM Category WHERE id = @id", new { id });
	}

	public List<Category> List()
	{
		using var conn = new SqlConnection(connectionString);
		return conn.Query<Category>("SELECT * FROM Category").ToList();
	}

	public void Add(Category category)
	{
		using var conn = new SqlConnection(connectionString);
		conn.Execute("INSERT INTO Category (name, image, parent_category_id) VALUES (@Name, @Image, @ParentCategoryId)", category);
	}

	public void Update(Category category)
	{
		using var conn = new SqlConnection(connectionString);
		conn.Execute("UPDATE Category SET name = @Name, image = @Image, parent_category_id = @ParentCategoryId WHERE id = @Id", category);
	}

	public void Delete(int id)
	{
		using var conn = new SqlConnection(connectionString);
		conn.Execute("DELETE FROM Category WHERE id = @id", new { id });
	}
}