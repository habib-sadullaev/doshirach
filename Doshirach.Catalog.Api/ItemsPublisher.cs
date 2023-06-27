using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Doshirach.Catalog.Core.Models;

namespace Doshirach.Catalog.Api;

public class Publisher
{
	private readonly string connectionString;
	private readonly ILogger<Publisher> logger;

	public Publisher(string connectionString, ILogger<Publisher> logger)
	{
		this.connectionString = connectionString;
		this.logger = logger;
	}

	public async Task ExecuteAsync(string method, Item item)
	{
		logger.LogInformation("Updateing item ...");

		await using var client = new ServiceBusClient(connectionString);
		await using var sender = client.CreateSender("Dosh");
		using var messageBatch = await sender.CreateMessageBatchAsync();

		var message = JsonSerializer.Serialize(new { Method = method, Item = item });

		if (!messageBatch.TryAddMessage(new ServiceBusMessage(message)))
			throw new InvalidOperationException($"The message {message} is too large to fit in the batch.");

		logger.LogInformation("Sending updated item message ...");

		await sender.SendMessagesAsync(messageBatch);
	}
}
