﻿using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", 
    durable: true, exclusive: false, autoDelete: false, arguments: null);

const string message = "Hello World";
var body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
Console.WriteLine(" [x] Sent {0}", message);

Console.WriteLine("Press [enter] to exit...");
Console.ReadLine();

