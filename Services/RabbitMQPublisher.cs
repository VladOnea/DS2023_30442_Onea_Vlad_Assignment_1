using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EnergyManagementSystem.Services
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly ConnectionFactory _factory;
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly string _queueName;

        public RabbitMQPublisher(string uri, string exchangeName, string routingKey, string queueName)
        {
            _factory = new ConnectionFactory { Uri = new Uri(uri) };
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _queueName = queueName;
        }

        public void PublishMessage<T>(T message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(_queueName, false, false, false, null);
            channel.QueueBind(_queueName, _exchangeName, _routingKey,null);
            var jsonMessage = JsonConvert.SerializeObject(message);
            var messageBodyBytes = Encoding.UTF8.GetBytes(jsonMessage);

            channel.BasicPublish(_exchangeName, _routingKey, null, messageBodyBytes);
            Console.WriteLine($"Sent: {jsonMessage}");
        }


    }

}
