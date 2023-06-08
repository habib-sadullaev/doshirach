using System.Data;
using Doshirach.Catalog.Api;
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

app.AddCategoryEndpints();
app.AddItemEndPoints();

app.Run();