using Doshirach.Catalog.Core.Models;
using Doshirach.Catalog.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Doshirach.Catalog.Api;

public static class Items
{
	public static void AddItemEndPoints(this WebApplication app)
	{
		app.MapGet("categories/{categoryId}/items", async ([FromServices] CatalogService catalogService, int categoryId, int? page, int? perPage) =>
		{
			var items = await catalogService.ListItemsAsync(categoryId, page ?? 1, perPage ?? 10);

			return from item in items
					 select new
					 {
						 item.CategoryId,
						 item.Id,
						 item.Name,
						 item.Description,
						 item.Price,
						 item.Amount,
						 Links = new[]
						 {
							 new { Rel = "self", Url = $"categories/{categoryId}/items/{item.Id}" },
							 new { Rel = "category", Url = $"categories/{categoryId}" },
						 },
					 };
		});

		app.MapGet("categories/{categoryId}/items/{id}", async ([FromServices] CatalogService catalogService, int categoryId, int id) =>
		{
			if (await catalogService.GetItemAsync(categoryId) is not { } item || item.CategoryId != categoryId)
				return Results.NotFound();

			return Results.Ok(new
			{
				item.CategoryId,
				item.Id,
				item.Name,
				item.Description,
				item.Price,
				item.Amount,
				Links = new[]
				{
					new { Rel = "self", Url = $"categories/{categoryId}/items/{item.Id}" },
					new { Rel = "category", Url = $"categories/{categoryId}" },
				},
			});
		});

		app.MapPost("categories/{categoryId}/items", async ([FromServices] CatalogService catalogService, Item item) =>
		{
			if (await catalogService.GetItemAsync(item.Id) is not null)
				return Results.Conflict();

			if (await catalogService.GetCategoryAsync(item.CategoryId) is null)
				return Results.BadRequest();

			await catalogService.AddItemAsync(item);

			return Results.NoContent();
		});

		app.MapPut("categories/{categoryId}/items/{id}", async (
			[FromServices] CatalogService catalogService,
			[FromServices] Publisher publisher,
			Item item) =>
		{
			await publisher.ExecuteAsync("PUT", item);
			if (await catalogService.GetItemAsync(item.Id) is not { } existingItem || existingItem.CategoryId != item.CategoryId)
				return Results.NotFound();

			await catalogService.UpdateItemAsync(item);
			return Results.NoContent();
		});

		app.MapDelete("categories/{categoryId}/items/{id}", async ([FromServices] CatalogService catalogService, int categoryId, int id) =>
		{
			if (await catalogService.GetItemAsync(id) is not { } item || item.CategoryId != categoryId)
				return Results.NotFound();

			await catalogService.DeleteItemAsync(id);

			return Results.NoContent();
		});
	}
}
