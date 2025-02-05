using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "hello", durable: true, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine(" [*] Waiting for messages...");

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message);
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queue: "hello", autoAck: true, consumer: consumer);

Console.WriteLine("Press [enter] to exit...");
Console.ReadLine();