using Doshirach.Catalog.Core.Models;

namespace Doshirach.Catalog.Core.Interfaces;

public interface IItemRepository
{
	Item Get(int id);
	List<Item> List();
	void Add(Item item);
	void Update(Item item);
	void Delete(int id);
}
