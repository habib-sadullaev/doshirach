using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Core.Models;

namespace Doshirach.Catalog.Domain;

public class CatalogService
{
	private readonly ICategoryRepository categoryRepository;
	private readonly IItemRepository itemRepository;

	public CatalogService(ICategoryRepository categoryRepository, IItemRepository itemRepository)
	{
		this.categoryRepository = categoryRepository;
		this.itemRepository = itemRepository;
	}

	public async Task<Category?> GetCategoryAsync(int categoryId)
	{
		if (categoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		return await categoryRepository.GetAsync(categoryId);
	}

	public async Task<Category[]> ListCategoriesAsync()
	{
		return await categoryRepository.ListAsync();
	}

	public Task AddCategoryAsync(Category category)
	{
		if (category.Id <= 0)
			throw new InvalidOperationException("Invalid category id");

		if (string.IsNullOrWhiteSpace(category.Name))
			throw new InvalidOperationException("Category name cannot be empty");

		if (category.Name.Length > 50)
			throw new InvalidOperationException("Category name length should be less than 50 characters");

		if (category.ParentCategoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		return categoryRepository.AddAsync(category);
	}

	public Task UpdateCategoryAsync(Category category)
	{
		if (category.Id <= 0)
			throw new InvalidOperationException("Invalid category id");

		if (string.IsNullOrWhiteSpace(category.Name))
			throw new InvalidOperationException("Category name cannot be empty");

		if (category.ParentCategoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		return categoryRepository.UpdateAsync(category);
	}

	public Task DeleteCategoryAsync(int categoryId)
	{
		if (categoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		return categoryRepository.DeleteAsync(categoryId);
	}

	public async Task<Item?> GetItemAsync(int itemId)
	{
		if (itemId <= 0)
			throw new InvalidOperationException("Invalid item id");

		return await itemRepository.GetAsync(itemId);
	}

	public async Task<Item[]> ListItemsAsync(int categoryId, int page, int pageSize)
	{
		return await itemRepository.ListAsync(categoryId, page, pageSize);
	}

	public Task AddItemAsync(Item item)
	{
		if (item.Id <= 0)
			throw new InvalidOperationException("Invalid item id");

		if (string.IsNullOrWhiteSpace(item.Name))
			throw new InvalidOperationException("Item name cannot be empty");

		if (item.Name.Length > 50)
			throw new InvalidOperationException("Item name length should be less than 50 characters");

		if (item.CategoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		if (item.Price <= 0)
			throw new InvalidOperationException("Item price must be positive");

		if (item.Amount <= 0)
			throw new InvalidOperationException("Item amount must be positive");

		return itemRepository.AddAsync(item);
	}

	public Task UpdateItemAsync(Item item)
	{
		if (item.Id <= 0)
			throw new InvalidOperationException("Invalid item id");

		if (string.IsNullOrWhiteSpace(item.Name))
			throw new InvalidOperationException("Item name cannot be empty");

		if (item.Name.Length > 50)
			throw new InvalidOperationException("Item name length should be less than 50 characters");

		if (item.CategoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		if (item.Price <= 0)
			throw new InvalidOperationException("Item price must be positive");

		if (item.Amount <= 0)
			throw new InvalidOperationException("Item amount must be positive");

		return itemRepository.UpdateAsync(item);
	}

	public Task DeleteItemAsync(int id)
	{
		if (id <= 0)
			throw new InvalidOperationException("Invalid item id");

		return itemRepository.DeleteAsync(id);
	}
}