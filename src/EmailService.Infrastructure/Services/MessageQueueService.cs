using EmailService.Domain.Configuration;
using EmailService.Domain.Services;
using MediatR;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;

namespace EmailService.Infrastructure.Services
{
    public class MessageQueueService : IMessageQueueService
    {

        private readonly MessageQueueConfiguration _settings;
        private IModel _channel;
        private readonly IMediator _mediator;

        public MessageQueueService(ConnectionFactory factory, IMediator mediator, MessageQueueConfiguration settings)
        {
            _mediator = mediator;
            _settings = settings;

            try
            {
                var retry = Policy.Handle<Exception>()
               .WaitAndRetry(new TimeSpan[]
               {
                    TimeSpan.FromSeconds(6),
                    TimeSpan.FromSeconds(12),
                    TimeSpan.FromSeconds(24)
               });

                retry.Execute(() =>
                {
                    var connection = factory.CreateConnection();
                    _channel = connection.CreateModel();
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public virtual void PostEventProcessing(params string[] args)
        {
            return;
        }

        public virtual void PreEventProcessing(params string[] args)
        {
            return;
        }

        public void ProcessEvent<T>()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var @event = JsonConvert.DeserializeObject<T>(content);
                PreEventProcessing();
                await _mediator.Send(@event);
                PostEventProcessing();
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            try
            {
                consumer.Model.QueueDeclare(_settings.QueueName, true, false);
                _channel.BasicConsume(_settings.QueueName, false, consumer);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
