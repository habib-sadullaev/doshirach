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

	public Category GetCategory(int categoryId)
	{
		if (categoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		return categoryRepository.Get(categoryId) ?? throw new InvalidOperationException("Invalid category id");
	}

	public IReadOnlyCollection<Category> ListCategories()
	{
		return categoryRepository.List();
	}

	public void AddCategory(Category category)
	{
		if (category.Id <= 0)
			throw new InvalidOperationException("Invalid category id");

		if (string.IsNullOrWhiteSpace(category.Name))
			throw new InvalidOperationException("Category name cannot be empty");

		if (category.ParentCategoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		categoryRepository.Add(category);
	}

	public void UpdateCategory(Category category)
	{
		if (category.Id <= 0)
			throw new InvalidOperationException("Invalid category id");

		if (string.IsNullOrWhiteSpace(category.Name))
			throw new InvalidOperationException("Category name cannot be empty");

		if (category.ParentCategoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		categoryRepository.Update(category);
	}

	public void DeleteCategory(int categoryId)
	{
		if (categoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		categoryRepository.Delete(categoryId);
	}

	public Item GetItem(int itemId)
	{
		if (itemId <= 0)
			throw new InvalidOperationException("Invalid item id");

		return itemRepository.Get(itemId) ?? throw new InvalidOperationException("Invalid item id");
	}

	public IReadOnlyCollection<Item> ListItems()
	{
		return itemRepository.List();
	}

	public void AddItem(Item item)
	{
		if (item.Id <= 0)
			throw new InvalidOperationException("Invalid item id");

		if (string.IsNullOrWhiteSpace(item.Name))
			throw new InvalidOperationException("Item name cannot be empty");

		if (item.CategoryId <= 0)
			throw new InvalidOperationException("Invalid category id");

		if (item.Price <= 0)
			throw new InvalidOperationException("Item price must be positive");

		if (item.Amount <= 0)
			throw new InvalidOperationException("Item amount must be positive");

		itemRepository.Add(item);
	}
}