using Doshirach.Catalog.Core.Models;

namespace Doshirach.Catalog.Core.Interfaces;

public interface ICategoryRepository
{
	Category Get(int id);
	List<Category> List();
	void Add(Category category);
	void Update(Category category);
	void Delete(int id);
}
