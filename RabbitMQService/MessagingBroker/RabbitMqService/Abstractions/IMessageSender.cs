using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqService.Abstractions
{
    public interface IMessageSender
    {
        Task PublishAsync<T>(T message, int priority = 1) where T : class;
        Task PublishAsync<J,K>(K message, int priority = 1) where J : class;
    }
}
