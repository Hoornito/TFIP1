using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqService.Abstractions
{
    public interface IMessagingBuilder
    {
        IServiceCollection Services { get; }
    }
}
