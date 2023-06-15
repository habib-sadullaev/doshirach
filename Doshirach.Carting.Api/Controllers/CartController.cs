using Doshirach.Carting.Core.Models;
using Doshirach.Carting.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doshirach.Carting.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class CartController : ControllerBase
{
	private readonly CartService cartService;

	public CartController(CartService cartService)
	{
		this.cartService = cartService;
	}

	[HttpGet("{cartKey}")]
	[MapToApiVersion("1.0")]
	public ActionResult GetCart(string cartKey)
		=> int.TryParse(cartKey, out var cartId)
			? Ok(new
			{
				CartKey = cartKey,
				CartItems =
					from cartItem in cartService.GetCartItems(cartId)
					select new
					{
						ItemKey = cartItem.Item.Id.ToString(),
						cartItem.Item.Name,
						cartItem.Item.Image,
						cartItem.Item.Price,
						cartItem.Quantity,
						cartItem.Item.Amount,
					}
			})
			: NotFound();

	[HttpGet("{cartKey}/items")]
	[MapToApiVersion("2.0")]
	public ActionResult GetCartItems(string cartKey)
		=> int.TryParse(cartKey, out var cartId)
			? Ok(from cartItem in cartService.GetCartItems(cartId)
				  select new
				  {
					  ItemKey = cartItem.Item.Id.ToString(),
					  cartItem.Item.Name,
					  cartItem.Item.Image,
					  cartItem.Item.Price,
					  cartItem.Quantity,
					  cartItem.Item.Amount,
				  })
			: Ok(Array.Empty<CartItem>());

	[HttpPost("{cartKey}/items/{itemKey}")]
	public ActionResult<CartItem> AddCartItem(string cartKey, string itemKey, Item item, int quantity)
	{
		if (!int.TryParse(cartKey, out var cartId))
			return BadRequest();

		if (!int.TryParse(itemKey, out var itemId))
			return BadRequest();

		var cartItem = cartService.AddCartItem(cartId, item with { Id = itemId }, quantity);

		return Ok(cartItem);
	}

	[HttpDelete("{cartKey}/items/{itemKey}")]
	public ActionResult DeleteCartItem(string cartKey, string itemKey)
	{
		if (!int.TryParse(cartKey, out var cartId))
			return NotFound();

		if (!int.TryParse(itemKey, out var itemId))
			return NotFound();

		if (!cartService.RemoveCartItem(cartId, itemId))
			return NotFound();

		return Ok();
	}
}