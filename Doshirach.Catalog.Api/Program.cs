using System.Data;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Core.Models;
using Doshirach.Catalog.Domain;
using Doshirach.Catalog.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbConnection>(_ => new SqliteConnection("Data Source=catalog.db"));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<CatalogService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

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
				 }
			 };
});

app.MapGet("/categories/{id}", async ([FromServices] CatalogService catalogService, int id) =>
{
	if (await catalogService.GetCategoryAsync(id) is var category && category is null)
		Results.NotFound();

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
		}
	});
});

app.MapPost("/categories", async ([FromServices] CatalogService catalogService, Category category) =>
{
	if (await catalogService.GetCategoryAsync(category.Id) is not null)
		return Results.Conflict();

	await catalogService.AddCategoryAsync(category);

	return Results.Created($"/categories/{category.Id}", category);
});

app.MapPut("/categories/{id}", async ([FromServices] CatalogService catalogService, Category category) =>
{
	if (await catalogService.GetCategoryAsync(category.Id) is null)
		return Results.NotFound();

	await catalogService.UpdateCategoryAsync(category);

	return Results.NoContent();
});

app.MapDelete("/categories/{id}", async ([FromServices] CatalogService catalogService, int id) =>
{
	if (await catalogService.GetCategoryAsync(id) is null)
		return Results.NotFound();

	await catalogService.DeleteCategoryAsync(id);

	return Results.NoContent();
});

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
				 }
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
			new { Rel = "category", Url = $"categories/{categoryId}" }
		}
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

app.MapPut("categories/{categoryId}/items/{id}", async ([FromServices] CatalogService catalogService, Item item) =>
{
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

app.Run();