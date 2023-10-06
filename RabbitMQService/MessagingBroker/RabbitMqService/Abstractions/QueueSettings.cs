using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqService.Abstractions
{
    public class QueueSettings
    {
        public IList<(string Name, Type Type)> Queues { get; } = new List<(string, Type)>();
        public IList<string>? QueuesString { get; } = new List<string>();

        public void Add<T>(string? queueName = null) where T : class
        {
            var type = typeof(T);
            Queues.Add((queueName ?? type.FullName, type));
        }
        public void Add(string? queueName = null)
        {
            QueuesString?.Add(queueName?? String.Empty);
        }

        //public void Add<T>(string? queueName = null, string ) where T : Type
        //{
        //    QueuesString?.Add(queueName ?? String.Empty);
        //}
    }
}
