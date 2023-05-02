using Doshirach.Carting.Common.Models;

namespace Doshirach.Carting.Common.Interfaces;

public interface ICartRepository
{
	Cart CreateCart(int cartId);
	Cart? GetCart(int cartId);
}
