using Microsoft.AspNetCore.Mvc;

using Shared.Models;
using RabbitMqService.Queues;
using RabbitMqService.RabbitMq;

using System.Diagnostics;
using System.Reflection.Metadata;
using Shared.Repositories;
using RabbitMqService.Abstractions;

namespace APIDocumentos.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IMessageSender _messageSender;
        private readonly IDocumentRepository _documentRepository; 

        public DocumentController(ILogger<DocumentController> logger, IDocumentRepository documentRepository, IMessageSender messageSender)
        {
            _documentRepository = documentRepository;
            _messageSender = messageSender;
            _logger = logger;
        }

        [HttpPost]
        [Route("SendDocument")]
        public async Task<IActionResult> SendDocument(IFormFile document, int priority)
        {
            try
            {
                DocumentRequest documentRequest = new DocumentRequest();
                documentRequest.Document = document;
                documentRequest.InsertDate = DateTime.Now;
                documentRequest.Status = "";
                
                string test = "test";

                await _messageSender.PublishAsync<Documentos, string>(test, priority);

                _logger.LogInformation($"Document sent to print.");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing the document.");
                return StatusCode(500, "Error processing hte document.");
            }
        }

        [HttpGet]
        [Route("GetDocument")]
        public async Task<IActionResult> GetDocument(string name)
        {
            try
            {
                var result = _documentRepository.GetDocument(name);

                _logger.LogInformation($"Document obtained.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Document not found.");
                return StatusCode(500, "Document not found.");
            }
        }
    }
}