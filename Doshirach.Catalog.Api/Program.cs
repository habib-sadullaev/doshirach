using System.Data;
using System.Security.Claims;
using Doshirach.Catalog.Api;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Domain;
using Doshirach.Catalog.Persistence;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
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
		options.DefaultScheme = "Cookies";
		options.DefaultChallengeScheme = "oidc";
		options.DefaultSignOutScheme = "oidc";
	})
	.AddCookie("Cookies")
	.AddOpenIdConnect("oidc", options =>
	{
		options.Authority = "https://localhost:5001";

		options.ClientId = "catalog";
		options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";
		options.ResponseType = "code";

		options.Scope.Add("openid");
		options.Scope.Add("profile");
		options.Scope.Add("offline_access");
		options.Scope.Add("api1");
		options.Scope.Add("role");

		options.GetClaimsFromUserInfoEndpoint = true;
		options.SaveTokens = true;

		options.ClaimActions.Add(new JsonKeyClaimAction("role", "role", "role"));

		options.TokenValidationParameters = new TokenValidationParameters
		{
			NameClaimType = "name",
			RoleClaimType = "role"
		};

		//// ignore self-signed ssl
		//options.BackchannelHttpHandler = new HttpClientHandler
		//{
		//	ServerCertificateCustomValidationCallback = (_, _, _, _) => true
		//};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Manager", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireRole("Manager");
	});
	options.AddPolicy("Buyer", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireRole("Buyer");
	});
});

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

app.MapGet("/Account/AccessDenied", () => "Access denied");

app.Run();
