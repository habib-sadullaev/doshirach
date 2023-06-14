using System.Data;
using System.Security.Claims;
using Doshirach.Catalog.Api;
using Doshirach.Catalog.Core.Interfaces;
using Doshirach.Catalog.Domain;
using Doshirach.Catalog.Persistence;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
	option.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog API", Version = "v1" });
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id="Bearer"
				}
			},
			new string[] { }
		}
	});
});
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
	});

builder.Services.AddAuthorization();

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
