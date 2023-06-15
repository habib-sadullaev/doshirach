using Doshirach.Carting.Core.Interfaces;
using Doshirach.Carting.Domain.Services;
using Doshirach.Carting.Grpc.Services;
using Doshirach.Carting.Persistence;
using LiteDB;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddGrpc();
services.AddSingleton<ILiteDatabase>(_ => new LiteDatabase(configuration.GetConnectionString("LiteDb")));
services.AddSingleton<ICartItemRepository, CartItemRepository>();
services.AddSingleton<CartService>();
var app = builder.Build();

app.MapGrpcService<GrpcCartService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
