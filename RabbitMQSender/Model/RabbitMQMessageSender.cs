using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQSender.Model
{
    public class RabbitMQMessageSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;
        public RabbitMQMessageSender()
        {
            _hostname = "localhost";
            _password = "guest";
            _userName = "guest";
        }

        public void SendMessage(Report message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _userName,
                Password = _password
            };
            try
            {
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            

            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, false, false, false, arguments: null);
                byte[] body = GetMessageAsByteArray(message);
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }

        }

        private byte[] GetMessageAsByteArray(Report message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}
