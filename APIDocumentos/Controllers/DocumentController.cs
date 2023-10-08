using Microsoft.AspNetCore.Mvc;

using Shared.Models;
using RabbitMqService.Queues;
using RabbitMqService.RabbitMq;

using System.Diagnostics;
using System.Reflection.Metadata;
using DAL.Repositories;
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
        public async Task<IActionResult> SendDocument(int priority, IFormFile? document)
        {
            try
            {
                if (document == null || document.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                using (var memoryStream = new MemoryStream())
                {
                    await document.CopyToAsync(memoryStream);
                    var documentContent = memoryStream.ToArray();

                    // Create a DocumentRequest instance and populate its properties
                    var documentRequest = new DocumentRequest
                    {
                        DocumentContent = documentContent,
                        ContentType = document.ContentType,
                        ContentDisposition = document.ContentDisposition,
                        FileName = document.FileName,
                        InsertDate = DateTime.Now,
                        Status = ""
                    };

                    // Now you can serialize and send the DocumentRequest
                    await _messageSender.PublishAsync<Documentos, DocumentRequest>(documentRequest, priority);

                    _logger.LogInformation($"Document sent to print.");

                    return Ok();
                }
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
                var result = await _documentRepository.GetDocument(name);
                if (result == null)
                {
                    _logger.LogInformation($"Document not printed.");
                    return NotFound("Document not printed.");
                }
                _logger.LogInformation($"Document obtained.");

                return Ok($"Document printed correctly. Document: {result.Name}, Insert Date: {result.InsertDate}, Print Date: {result.PrintDate}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Document not printed.");
                return StatusCode(500, "Document not printed.");
            }
        }
    }
}