using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RabbitMqService.Abstractions;
using RabbitMqService.Queues;

using Shared.Models;

using System.Text;
using System.Text.Json;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SistemaReceptor.Services
{
    public class DocumentService : IMessageReceiver<object>
    {
        private readonly ILogger _logger;
        private readonly IMessageSender _messageSender;

        public DocumentService(ILogger<DocumentService> logger, IMessageSender messageSender) 
        {
            _logger = logger;
            _messageSender = messageSender;
        }

        public async Task ReceiveAsync(object document, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Message received to process a document.");

                Random random = new Random();
                //int randomNumber = random.Next(1, 4);
                int randomNumber = 4;

                if (randomNumber == 1 || randomNumber == 2)
                {
                    DocumentRequest jsonData = JsonConvert.DeserializeObject<DocumentRequest>(document.ToString());
                    string fileName = jsonData.FileName;
                    byte[] fileContent = ((JToken)jsonData.DocumentContent).ToObject<byte[]>();

                    IFormFile formFile = new FormFile(new MemoryStream(fileContent), 0, fileContent.Length, "file", fileName);

                    DocumentInfo documentInfo = new DocumentInfo
                    {
                        Name = fileName,
                        InsertDate = jsonData.InsertDate,
                        PrintDate = DateTime.Now,
                        Status = "Ok"
                    };

                    await _messageSender.PublishAsync<Respuestas, DocumentInfo>(documentInfo);

                    _logger.LogInformation($"Document processed.");
                }
                else
                {
                    _logger.LogInformation("No response for the document.");
                    throw new Exception("No response for the document.");
                    // Simula que no responde.
                }
            }
            catch (Exception)
            {

                throw;
            }            
        }
    }
}
