using Doshirach.Catalog.Core.Models;
using Doshirach.Catalog.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Doshirach.Catalog.Api;

public static class Categories
{
	public static void AddCategoryEndpints(this WebApplication app)
	{
		app.MapGet("/categories", async ([FromServices] CatalogService catalogService) =>
		{
			var categories = await catalogService.ListCategoriesAsync();

			return from category in categories
					 select new
					 {
						 category.Id,
						 category.Image,
						 category.Name,
						 category.ParentCategoryId,
						 Links = new[]
						 {
							 new { Rel = "self", Uri = $"/categories/{category.Id}" },
							 new { Rel = "category items", Uri = $"/categories/{category.Id}/items" },
						 },
					 };
		}).RequireAuthorization("Manager", "Buyer");

		app.MapPost("/categories", async ([FromServices] CatalogService catalogService, Category category) =>
		{
			if (await catalogService.GetCategoryAsync(category.Id) is not null)
				return Results.Conflict();

			await catalogService.AddCategoryAsync(category);

			return Results.Created($"/categories/{category.Id}", category);
		}).RequireAuthorization("Manager");

		app.MapGet("/categories/{id}", async ([FromServices] CatalogService catalogService, int id) =>
		{
			if (await catalogService.GetCategoryAsync(id) is not { } category)
				return Results.NotFound();

			return Results.Ok(new
			{
				category.Id,
				category.Image,
				category.Name,
				category.ParentCategoryId,
				Links = new[]
				{
					new { Rel = "self", Uri = $"/categories/{category.Id}" },
					new { Rel = "items", Uri = $"/categories/{category.Id}/items" },
				},
			});
		}).RequireAuthorization("Manager", "Buyer");

		app.MapPut("/categories/{id}", async ([FromServices] CatalogService catalogService, Category category) =>
		{
			if (await catalogService.GetCategoryAsync(category.Id) is null)
				return Results.NotFound();

			await catalogService.UpdateCategoryAsync(category);

			return Results.NoContent();
		}).RequireAuthorization("Manager");

		app.MapDelete("/categories/{id}", async ([FromServices] CatalogService catalogService, int id) =>
		{
			if (await catalogService.GetCategoryAsync(id) is null)
				return Results.NotFound();

			await catalogService.DeleteCategoryAsync(id);

			return Results.NoContent();
		}).RequireAuthorization("Manager");
	}
}
