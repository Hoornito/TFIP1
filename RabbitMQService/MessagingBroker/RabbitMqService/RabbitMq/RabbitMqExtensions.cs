using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMqService.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqService.RabbitMq
{

    public static class RabbitMQExtensions
    {
        public static IMessagingBuilder AddRabbitMq(this IServiceCollection services, Action<MessageManagerSettings> messageManagerConfiguration, Action<QueueSettings> queuesConfiguration)
        {
            services.AddSingleton<MessageManager>();
            services.AddSingleton<IMessageSender>(x => x.GetRequiredService<MessageManager>());
            services.AddSingleton<IMessageSender>(provider => provider.GetService<MessageManager>());

            var messageManagerSettings = new MessageManagerSettings();
            messageManagerConfiguration.Invoke(messageManagerSettings);
            services.AddSingleton(messageManagerSettings);

            var queueSettings = new QueueSettings();
            queuesConfiguration.Invoke(queueSettings);
            services.AddSingleton(queueSettings);

            var messageManager = new MessageManager(messageManagerSettings,queueSettings);

            return new DefaultMessagingBuilder(services);
        }

        /// <summary>
        /// Método para registrar un HostedService
        /// </summary>
        /// <typeparam name="TListener"></typeparam>
        /// <typeparam name="TObject"></typeparam>
        /// <typeparam name="TReceiver"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMessagingBuilder AddReceiver<TListener, TObject, TReceiver>(this IMessagingBuilder builder)
            where TListener : BackgroundService
            where TObject : class
            where TReceiver : class, IMessageReceiver<TObject>
        {
            builder.Services.AddHostedService<TListener>();
            builder.Services.AddTransient<IMessageReceiver<TObject>, TReceiver>();

            return builder;
        }
    }
}
