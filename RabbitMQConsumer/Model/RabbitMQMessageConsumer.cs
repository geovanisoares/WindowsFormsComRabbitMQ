using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQConsumer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQConsumer.Model
{
    public class RabbitMQMessageConsumer: BackgroundService
    {
        DbContextOptions<Context> _options;
        private IConnection _connection;
        private IModel _channel;
        
        public RabbitMQMessageConsumer()
        {
            _options = new DbContextOptionsBuilder<Context>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
                
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "LeadQueue", false, false, false, arguments: null);

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);



            consumer.Received += (channel, evt) =>
            {
                
                var content = Encoding.UTF8.GetString(evt.Body.ToArray());
                Report report = JsonSerializer.Deserialize<Report>(content);
                Report teste = ProcessaReport(report).GetAwaiter().GetResult();
                



            };
            _channel.BasicConsume("LeadQueue", false, consumer);
            



            return Task.CompletedTask;
        }

        private async Task<Report> ProcessaReport(Report reportReceive)
        {
            Report report = new Report()
            {
                Id = reportReceive.Id,
                Nome = reportReceive.Nome,
                Idade = reportReceive.Idade,
                DataHora = reportReceive.DataHora
            };

            Console.WriteLine(report.Id + " - " + report.Nome);

            using (var context = new Context(_options))
            {
                Guid guid = Guid.NewGuid();
                
                await context.Reports.AddAsync(report);
                context.SaveChanges();

            }

            return report;

        }
    }
}
