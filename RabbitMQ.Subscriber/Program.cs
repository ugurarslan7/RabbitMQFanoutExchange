using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        ConnectionFactory factory = new ConnectionFactory();
        //factory.HostName = "localhost";
        //factory.VirtualHost = "/";
        //factory.Port = 5672;
        //factory.UserName = "guest";
        //factory.Password = "guest";

        factory.Uri = new Uri("amqps://jcabnccy:j5e8IUxSyGYiI93gEH0wYlJhfsuPNbQk@stingray.rmq.cloudamqp.com/jcabnccy");

        var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        var randomQueneName = channel.QueueDeclare().QueueName;

        channel.QueueBind(randomQueneName, "logs-fanout", "", null);

        channel.BasicQos(0, 1, false);
        
        var subscriber = new EventingBasicConsumer(channel);

        channel.BasicConsume(randomQueneName, false, subscriber);

        Console.WriteLine("Logs are listening...");

        subscriber.Received += (object? sender, BasicDeliverEventArgs e) =>
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            Thread.Sleep(1000);

            //Clear incoming message
            channel.BasicAck(e.DeliveryTag, false);

            Console.WriteLine(message);

        };

        Console.ReadLine();

    }

}
