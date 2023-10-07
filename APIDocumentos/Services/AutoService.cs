using Newtonsoft.Json;

using RabbitMqService.Abstractions;

using Shared.Models;
using Shared.Repositories;

using System.Runtime.InteropServices.ObjectiveC;

namespace APIDocumentos.Services
{
    public class AutoService : IMessageReceiver<DocumentInfo>
    {
        private readonly ILogger _logger;
        private readonly IDocumentRepository _documentRepository;

        public AutoService(ILogger<AutoService> logger, IDocumentRepository documentRepository)
        {
            _logger = logger;
            _documentRepository = documentRepository;
        }

        public async Task ReceiveAsync(DocumentInfo document, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Message received to process a document.");

                await _documentRepository.Insert(document);

                _logger.LogInformation($"Document processed.");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }   
    }
}
