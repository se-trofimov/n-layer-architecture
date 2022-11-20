using API;
using API.Filters;
using CatalogService.Application;
using CatalogService.Infrastructure;
using Messaging.Abstractions;
using Messaging.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using RabbitMQ.Client;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers(
            options =>
            {
                options.Filters.Add<ApiExceptionFilterAttribute>();
                options.Filters.Add<ValidateMediaTypeAttribute>();
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;

                options.Filters.Add(new ProducesAttribute("application/json", new[]{ "application/hateoas+json" }));
                options.Filters.Add(new ConsumesAttribute("application/json",new[]{ "application/hateoas+json" }));
                
            });

        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.Configure<MvcOptions>(config =>
        {
            var jsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

            if (jsonOutputFormatter != null)
            {
                jsonOutputFormatter.SupportedMediaTypes.Add("application/hateoas+json");
            }
        });
        
        builder.Services.AddScoped<ValidateMediaTypeAttribute>();
        builder.Services.AddScoped(typeof(IQueueProducer<>), typeof(RabbitQueueProducer<>));
        var rabbitMqConfig = new RabbitMqConfiguration();
        builder.Configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(rabbitMqConfig);

        builder.Services.AddSingleton<IConnectionFactory>(new ConnectionFactory()
        {
            HostName = rabbitMqConfig.HostName,
            Port = rabbitMqConfig.Port
        });
        
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
    }
}