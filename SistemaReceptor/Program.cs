using RabbitMqService.Queues;
using RabbitMqService.RabbitMq;

using Shared.AppSettings;
using Shared.Helpers;
using Shared.Models;

using APIReceptor.Receiver;
using APIReceptor.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);
// Add services to the container.


void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddScoped<DocumentService>();

    services.AddRabbitMq(settings =>
    {
        settings.ConnectionString = configuration.GetValue<string>("RabbitMq:ConnectionString");
        settings.ExchangeName = configuration.GetValue<string>("AppSettings:ApplicationName");
        settings.QueuePrefetchCount = configuration.GetValue<ushort>("AppSettings:QueuePrefetchCount");
    }, queues =>
    {
        queues.Add<Documentos>();
        queues.Add<Respuestas>();
    })
    .AddReceiver<DocumentReceiver<object>, object, DocumentService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
