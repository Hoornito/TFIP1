using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


using RabbitMqService.Abstractions;
using RabbitMqService.Queues;

using Shared.Models;


namespace Sistema_Receptor.Services
{
    public class DocumentService : IMessageReceiver<DocumentRequest>
    {
        private readonly ILogger _logger;
        private readonly IMessageSender _messageSender;

        public DocumentService(ILogger<DocumentService> logger, IMessageSender messageSender) 
        {
            _logger = logger;
            _messageSender = messageSender;
        }

        public async Task ReceiveAsync(DocumentRequest document, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message received to process a document.");

            Random random = new Random();
            int randomNumber = random.Next(1, 4);

            if (randomNumber == 1 || randomNumber == 2)
            {
                document.PrintDate = DateTime.Now;
                document.Status = "Ok";

                await _messageSender.PublishAsync<Respuestas, object>(document);

                _logger.LogInformation($"Document processed.");
            }
            else
            {
                // Simula que no responde.
                _logger.LogInformation("No response for the document.");
            }
            
        }
    }
}
