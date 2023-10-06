// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using RabbitMqService.Queues;
using RabbitMqService.RabbitMq;

using Shared.AppSettings;
using Shared.Repositories;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);