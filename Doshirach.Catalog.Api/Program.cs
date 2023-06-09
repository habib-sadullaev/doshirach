using System.Data;
using Doshirach.Catalog.Api;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Domain;
using Doshirach.Catalog.Persistence;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddScoped<IDbConnection>(_ => new SqliteConnection(configuration.GetConnectionString("Sqlite")));
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<IItemRepository, ItemRepository>();
services.AddScoped<CatalogService>();
services.AddSingleton(_ =>  new Publisher(configuration.GetConnectionString("ServiceBus")!));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.AddCategoryEndpints();
app.AddItemEndPoints();

app.Run();