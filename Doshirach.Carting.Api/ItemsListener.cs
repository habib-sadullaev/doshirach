using System.Net;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Doshirach.Carting.Core.Models;
using Doshirach.Carting.Domain.Services;

namespace Doshirach.Carting.Api;

public class ItemsListener : BackgroundService
{
	readonly CartService cartService;
	readonly ServiceBusClient client;
	readonly ServiceBusProcessor processor;

	public ItemsListener(CartService cartService, string connectionString)
	{
		this.cartService = cartService;
		client = new ServiceBusClient(connectionString);
		processor = client.CreateProcessor("Dosh");
		processor.ProcessMessageAsync += MessageHandler;
		processor.ProcessErrorAsync += ErrorHandler;
	}

	Task ErrorHandler(ProcessErrorEventArgs arg)
	{
		return Task.CompletedTask;
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		processor.StartProcessingAsync(stoppingToken);
		return Task.CompletedTask;
	}

	async Task MessageHandler(ProcessMessageEventArgs args)
	{
		var message = new { Method = default(string)!, Item = default(Item)! };
		var rawStream = args.Message.Body.ToStream();
		var messageType = message.GetType();
		
		message = await JsonSerializer.DeserializeAsync(rawStream, messageType) switch
		{
			null => throw new InvalidOperationException("Invalid message"),
			var value => (dynamic)value,
		};

		switch (message.Method)
		{
			case WebRequestMethods.Http.Put:
				cartService.UpdateItem(message.Item);
				break;
			case var method:
				throw new InvalidOperationException($"Unknown {nameof(method)} {method}");
		}

		await args.CompleteMessageAsync(args.Message);
	}

	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		await base.StopAsync(cancellationToken);

		await processor.StopProcessingAsync(cancellationToken);

		processor.ProcessMessageAsync -= MessageHandler;
		processor.ProcessErrorAsync -= ErrorHandler;

		await processor.DisposeAsync();
		await client.DisposeAsync();
	}
}
