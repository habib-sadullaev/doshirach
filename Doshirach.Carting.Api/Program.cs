using Doshirach.Carting.Api;
using Doshirach.Carting.Core.Interfaces;
using Doshirach.Carting.Domain.Services;
using Doshirach.Carting.Persistence;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddApplicationInsightsTelemetry();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddApiVersioning(options =>
{
	options.ReportApiVersions = true;
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new ApiVersion(1, 0);
});
services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});
services.AddOptions<SwaggerGenOptions>()
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

services.AddSwaggerGen();
services.AddSingleton<ILiteDatabase>(_ => new LiteDatabase(configuration.GetConnectionString("LiteDb")));
services.AddSingleton<ICartItemRepository, CartItemRepository>();
services.AddSingleton<CartService>();
services.AddHostedService(p => new ItemsListener(
	p.GetRequiredService<CartService>(), 
	configuration.GetConnectionString("ServiceBus")!));

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
LiteDbMapping.Configure();

app.Run();
