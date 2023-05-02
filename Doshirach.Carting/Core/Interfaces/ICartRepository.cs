using Doshirach.Carting.Core.Models;

namespace Doshirach.Carting.Core.Interfaces;

public interface ICartRepository
{
	Cart CreateCart(int cartId);
	Cart? GetCart(int cartId);
}
