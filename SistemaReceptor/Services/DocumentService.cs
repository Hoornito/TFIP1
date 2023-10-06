using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


using RabbitMqService.Abstractions;
using RabbitMqService.Queues;

using Shared.Models;


namespace SistemaReceptor.Services
{
    public class DocumentService : IMessageReceiver<string>
    {
        private readonly ILogger _logger;
        private readonly IMessageSender _messageSender;

        public DocumentService(ILogger<DocumentService> logger, IMessageSender messageSender) 
        {
            _logger = logger;
            _messageSender = messageSender;
        }

        public async Task ReceiveAsync(string document, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Message received to process a document.");

                Random random = new Random();
                int randomNumber = random.Next(1, 4);

                if (randomNumber == 1 || randomNumber == 2)
                {
                    //DocumentInfo documentInfo = new DocumentInfo();
                    //documentInfo.Name = document.Document.FileName;
                    //documentInfo.InsertDate = document.InsertDate;
                    //documentInfo.PrintDate = DateTime.Now;
                    //documentInfo.Status = "Ok";

                    await _messageSender.PublishAsync<Respuestas, string>("test");

                    _logger.LogInformation($"Document processed.");
                }
                else
                {
                    // Simula que no responde.
                    _logger.LogInformation("No response for the document.");
                }
            }
            catch (Exception)
            {

                throw;
            }            
        }
    }
}
