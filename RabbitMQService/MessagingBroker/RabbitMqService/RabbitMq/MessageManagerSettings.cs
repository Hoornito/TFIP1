using RabbitMqService.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMqService.RabbitMq
{
    public class MessageManagerSettings
    {
        public string ConnectionString { get; set; }

        public string ExchangeName { get; set; }

        public ushort QueuePrefetchCount { get; set; }

        public JsonSerializerOptions JsonSerializerOptions { get; set; } = JsonOptions.Default;
    }
}
