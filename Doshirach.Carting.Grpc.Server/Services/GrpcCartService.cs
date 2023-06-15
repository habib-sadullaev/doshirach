using Grpc.Core;
using CartServiceBase = Doshirach.Carting.Grpc.CartService.CartServiceBase;

namespace Doshirach.Carting.Grpc.Server.Services
{
	public class GrpcCartService : CartServiceBase
	{
		private readonly Domain.Services.CartService cartService;
		private readonly ILogger<GrpcCartService> logger;

		public GrpcCartService(Domain.Services.CartService cartService, ILogger<GrpcCartService> logger)
		{
			this.cartService = cartService;
			this.logger = logger;
		}

		public override Task<GetCartItemsResponse> GetCartItems(GetCartItemsRequest request, ServerCallContext context)
		{
			logger.LogInformation("receiving cart items...");
			return Task.FromResult(new GetCartItemsResponse
			{
				CartItems =
				{
					from cartItem in cartService.GetCartItems(request.CartId)
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
					}
				}
			});
		}
	}
}