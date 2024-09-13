using RabbitMQ.Client;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory();

        factory.Uri = new Uri("amqps://jcabnccy:j5e8IUxSyGYiI93gEH0wYlJhfsuPNbQk@stingray.rmq.cloudamqp.com/jcabnccy");

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.ExchangeDeclare("logs-fanout", durable: true, type: ExchangeType.Fanout);

        Enumerable.Range(0, 50).ToList().ForEach(Range =>
        {
            string message = Range.ToString() + " .Message";

            var messageBody = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("logs-fanout", "", null, messageBody);

            Console.WriteLine(message);
        });

        Console.ReadLine();

    }
}
