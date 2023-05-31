using Doshirach.Catalog.Core.Models;

namespace Doshirach.Catalog.Core.Interfaces;

public interface IItemRepository
{
	Task<Item?> GetAsync(int id);
	Task<Item[]> ListAsync(int categoryId, int page, int pageSize);
	Task AddAsync(Item item);
	Task UpdateAsync(Item item);
	Task DeleteAsync(int id);
}
