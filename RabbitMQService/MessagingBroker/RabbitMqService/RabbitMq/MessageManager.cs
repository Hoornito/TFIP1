using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqService.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMqService.RabbitMq
{
    public class MessageManager : IMessageSender, IDisposable
    {
        public const string MaxPriorityHeader = "x-max-priority";

        public IConnection Connection { get; set; }

        public IModel Channel { get; set; }

        private readonly MessageManagerSettings messageManagerSettings;
        private readonly QueueSettings queueSettings;

        public MessageManager(MessageManagerSettings messageManagerSettings, QueueSettings queueSettings)
        {
            var factory = new ConnectionFactory { HostName = messageManagerSettings.ConnectionString };
            Connection = factory.CreateConnection();

            Channel = Connection.CreateModel();

            if (messageManagerSettings.QueuePrefetchCount > 0)
            {
                Channel.BasicQos(0, messageManagerSettings.QueuePrefetchCount, false);
            }

            Channel.ExchangeDeclare(messageManagerSettings.ExchangeName, ExchangeType.Direct, durable: true);

            if (queueSettings.Queues.Count > 0)
            {

                foreach (var queue in queueSettings.Queues)
                {
                    var args = new Dictionary<string, object>
                    {
                        [MaxPriorityHeader] = 10
                    };
                    
                    Channel.QueueDeclare(queue.Type.Name ?? queue.Name, durable: true, exclusive: false, autoDelete: false, args);
                    Channel.QueueBind(queue.Type.Name ?? queue.Name, messageManagerSettings.ExchangeName, queue.Type.Name ?? queue.Name, null);
                }
            }
            //else
            //{
            //    foreach (var queue in queueSettings.QueuesString)
            //    {
            //        var args = new Dictionary<string, object>
            //        {
            //            [MaxPriorityHeader] = 10
            //        };

            //        Channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, args);
            //        Channel.QueueBind(queue, messageManagerSettings.ExchangeName, queue, null);
            //    }
            //}


            this.messageManagerSettings = messageManagerSettings;
            this.queueSettings = queueSettings;
        }

        public Task PublishAsync<T>(T message, int priority = 1) where T : class
        {
            var sendBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<object>(message, messageManagerSettings.JsonSerializerOptions ?? JsonOptions.Default));
            var routingKey = queueSettings.Queues.FirstOrDefault(q => q.Type == typeof(T)).Type.Name ?? queueSettings?.QueuesString?.FirstOrDefault();
            return PublishAsync(sendBytes.AsMemory(), routingKey ?? string.Empty, priority);
        }

        public Task PublishAsync<J, K>(K message, int priority = 1) where J : class
        {
            var sendBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize<object>(message, messageManagerSettings.JsonSerializerOptions ?? JsonOptions.Default));
            var routingKey = queueSettings.Queues.FirstOrDefault(q => q.Type == typeof(J)).Type.Name ?? queueSettings?.QueuesString?.FirstOrDefault();
            return PublishAsync(sendBytes.AsMemory(), routingKey ?? string.Empty, priority);
        }

        private Task PublishAsync(ReadOnlyMemory<byte> body, string routingKey, int priority = 1)
        {
            var properties = Channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Priority = Convert.ToByte(priority);

            Channel.BasicPublish(messageManagerSettings.ExchangeName, routingKey, properties, body);
            return Task.CompletedTask;
        }

        public void MarkAsComplete(BasicDeliverEventArgs message) => Channel.BasicAck(message.DeliveryTag, false);

        public void MarkAsRejected(BasicDeliverEventArgs message) => Channel.BasicReject(message.DeliveryTag, false);

        public void Dispose()
        {
            try
            {
                if (Channel.IsOpen)
                {
                    Channel.Close();
                }

                if (Connection.IsOpen)
                {
                    Connection.Close();
                }
            }
            catch
            {
            }

            GC.SuppressFinalize(this);
        }
    }
}
