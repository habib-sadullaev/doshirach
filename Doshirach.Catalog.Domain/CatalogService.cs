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

	public Category GetCategory(int id)
	{
		return categoryRepository.Get(id);
	}

	public List<Category> ListCategories()
	{
		return categoryRepository.List();
	}

	public void AddCategory(Category category)
	{
		categoryRepository.Add(category);
	}

	public void UpdateCategory(Category category)
	{
		categoryRepository.Update(category);
	}

	public void DeleteCategory(int id)
	{
		categoryRepository.Delete(id);
	}

	public Item GetItem(int id)
	{
		return itemRepository.Get(id);
	}

	public List<Item> ListItems()
	{
		return itemRepository.List();
	}

	public void AddItem(Item item)
	{
		itemRepository.Add(item);
	}
}