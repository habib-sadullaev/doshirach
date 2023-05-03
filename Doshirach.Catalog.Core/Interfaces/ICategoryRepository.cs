using Doshirach.Catalog.Core.Models;

namespace Doshirach.Catalog.Core.Interfaces;

public interface ICategoryRepository
{
	Task<Category?> GetAsync(int id);
	Task<Category[]> ListAsync();
	Task AddAsync(Category category);
	Task UpdateAsync(Category category);
	Task DeleteAsync(int id);
}
