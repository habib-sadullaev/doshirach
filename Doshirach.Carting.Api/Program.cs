using Doshirach.Carting.Core.Interfaces;
using Doshirach.Carting.Domain.Services;
using Doshirach.Carting.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
	options.ReportApiVersions = true;
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new ApiVersion(1, 0);
});
builder.Services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddOptions<SwaggerGenOptions>()
	.Configure((SwaggerGenOptions options, IApiVersionDescriptionProvider provider) =>
	{
		foreach (var description in provider.ApiVersionDescriptions)
		{
			options.SwaggerDoc(description.GroupName, new OpenApiInfo
			{
				Title = $"Carting Service API {description.ApiVersion}",
				Version = description.ApiVersion.ToString()
			});
		}
	});
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICartRepository>(_ => new CartRepository(builder.Configuration.GetConnectionString("LiteDb")!));
builder.Services.AddScoped<ICartItemRepository>(_ => new CartItemRepository(builder.Configuration.GetConnectionString("LiteDb")!));
builder.Services.AddScoped<CartService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
		foreach (var description in apiVersionProvider.ApiVersionDescriptions.Reverse())
		{
			options.SwaggerEndpoint(
				$"/swagger/{description.GroupName}/swagger.json",
				description.GroupName.ToUpperInvariant());
		}
	});
}

app.MapControllers();

app.Run();
