using Newtonsoft.Json;
using RabbitMQ.Client;
using RestApiRepo.Domain.Configurations;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.Services;
using System;
using System.Text;

namespace RestApiRepo.Infrastructure.Services
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly MessageQueueConfiguration _messageQueueConfiguration;
        private readonly ConnectionFactory _connectionFactory;

        public MessageQueueService(ConnectionFactory connectionFactory, MessageQueueConfiguration messageQueueConfiguration)
        {
            _connectionFactory = connectionFactory;
            _messageQueueConfiguration = messageQueueConfiguration;
        }

        public bool SendMessage(IMessageQueueMessage message)
        {
            try
            {
                var connection = _connectionFactory.CreateConnection();

                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: _messageQueueConfiguration.QueueName, true, false);

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: _messageQueueConfiguration.QueueName, body: body);
                channel.WaitForConfirmsOrDie();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
