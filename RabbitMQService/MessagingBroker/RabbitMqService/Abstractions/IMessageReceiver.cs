using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqService.Abstractions
{
    public interface IMessageReceiver<T> where T : class
    {
        Task ReceiveAsync(T message, CancellationToken cancellationToken);
    }
}
