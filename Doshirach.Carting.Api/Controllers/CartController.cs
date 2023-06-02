using Doshirach.Carting.Core.Models;
using Doshirach.Carting.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Doshirach.Carting.Api.Controllers;

[ApiController]
//[Route("api/[controller]")]
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
	public ActionResult<Cart> GetCart(string cartKey)
		=> int.TryParse(cartKey, out var id) && cartService.GetCart(id) is { } cart
			? Ok(new
			{
				CartKey = id.ToString(),
				CartItems = from item in cart.Items
								select new
								{
									ItemKey = item.Id.ToString(),
									item.Name,
									item.Image,
									item.Price,
									item.Quantity,
								}
			})
			: NotFound();

	[HttpGet("{cartKey}/items")]
	[MapToApiVersion("2.0")]
	public ActionResult<CartItem[]> GetCartItems(string cartKey)
		=> int.TryParse(cartKey, out var id) && cartService.GetCartItems(id) is var cartItems
			? Ok(from item in cartItems
				  select new
				  {
					  ItemKey = item.Id.ToString(),
					  item.Name,
					  item.Image,
					  item.Price,
					  item.Quantity,
				  })
			: Ok(Array.Empty<CartItem>());

	[HttpPost("{cartKey}/items/{itemKey}")]
	public ActionResult AddCartItem(string cartKey, string itemKey, CartItem item)
	{
		if (!int.TryParse(cartKey, out var id))
			return BadRequest();

		if (!int.TryParse(itemKey, out var itemId))
			return BadRequest();

		item = item with { Id = itemId, CartId = id };
		if (!cartService.AddCartItem(item))
			return BadRequest();

		return Ok();
	}

	[HttpDelete("{cartKey}/items/{itemKey}")]
	public ActionResult DeleteCartItem(string cartKey, string itemKey)
	{
		if (!int.TryParse(cartKey, out _))
			return NotFound();

		if (!int.TryParse(itemKey, out var itemId))
			return NotFound();

		if (!cartService.RemoveCartItem(itemId))
			return NotFound();

		return Ok();
	}
}