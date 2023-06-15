using Grpc.Core;
using CartServiceBase = Doshirach.Carting.Grpc.CartService.CartServiceBase;

namespace Doshirach.Carting.Grpc.Server.Services;

public class GrpcCartService : CartServiceBase
{
	private readonly Domain.Services.CartService cartService;
	private readonly ILogger<GrpcCartService> logger;

	public GrpcCartService(Domain.Services.CartService cartService, ILogger<GrpcCartService> logger)
	{
		this.cartService = cartService;
		this.logger = logger;
	}

	public override Task<CartItemsResponse> GetCartItems(GetCartItemsRequest request, ServerCallContext context)
	{
		return Task.FromResult(GetCartItems(request.CartId, context.CancellationToken));
	}

	public override Task<CartItemsResponse> AddCartItem(AddCartItemRequest request, ServerCallContext context)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		logger.LogInformation("adding an item to the cart {cartId}...", request.CartItem.CartId);

		var cartItem = cartService.AddCartItem(
			request.CartItem.CartId,
			new()
			{
				Id = request.CartItem.Id,
				Name = request.CartItem.Name,
				Image = new(request.CartItem.Image.Url, request.CartItem.Image.Text),
				Price = request.CartItem.Price,
				Amount = request.CartItem.Amount,
			},
			request.CartItem.Quantity);

		logger.LogInformation("cartItem with cartId {cartId} and id {id} has been added",
			cartItem.CartId,
			cartItem.Item.Id);

		return Task.FromResult(GetCartItems(cartItem.CartId, context.CancellationToken));
	}

	private CartItemsResponse GetCartItems(int cartId, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		logger.LogInformation("receiving items in the cart {cartId}...", cartId);

		var cartItems =
			from cartItem in cartService.GetCartItems(cartId)
			select new CartItem
			{
				CartId = cartItem.CartId,
				Id = cartItem.Item.Id,
				Name = cartItem.Item.Name,
				Image = new()
				{
					Url = cartItem.Item.Image.Url,
					Text = cartItem.Item.Image.Text,
				},
				Price = cartItem.Item.Price,
				Amount = cartItem.Item.Amount,
				Quantity = cartItem.Quantity,
			};

		var response = new CartItemsResponse();

		foreach (var cartItem in cartItems)
		{
			cancellationToken.ThrowIfCancellationRequested();

			response.CartItems.Add(cartItem);
		}

		logger.LogInformation("cart {cartId} items have been received", cartId);

		return response;
	}
}