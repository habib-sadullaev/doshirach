using System.Data;
using System.Security.Claims;
using Doshirach.Catalog.Api;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Domain;
using Doshirach.Catalog.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddScoped<IDbConnection>(_ => new SqliteConnection("Data Source=catalog.db"));
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<IItemRepository, ItemRepository>();
services.AddScoped<CatalogService>();

services.AddAuthentication(options =>
	{
		options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultAuthenticateScheme = "oidc";
	})
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		options.Authority = "https://localhost:5001";
		options.TokenValidationParameters.ValidateAudience = false;
		options.RequireHttpsMetadata = false;
	})
	.AddOpenIdConnect("oidc", options =>
	{
		options.Authority = "https://localhost:5001";

		options.ClientId = "catalog";
		options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
		options.ResponseType = "code";

		options.Scope.Add("openid");
		options.Scope.Add("profile");
		options.Scope.Add("api1");

		options.GetClaimsFromUserInfoEndpoint = true;
		options.SaveTokens = true;

		options.TokenValidationParameters = new TokenValidationParameters
		{
			NameClaimType = "name",
			RoleClaimType = "role"
		};
	});

builder.Services.AddAuthorization(options =>
	options.AddPolicy("ApiScope", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireClaim("scope", "api1");
	})
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.AddCategoryEndpints();
app.AddItemEndPoints();
app
	.MapGet(
		"/identity",
		(ClaimsPrincipal user) => Results.Json(from c in user.Claims select new { c.Type, c.Value }))
	.RequireAuthorization("ApiScope");

app.Run();
