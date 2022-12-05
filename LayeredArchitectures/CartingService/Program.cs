using CartingService;
using System.Reflection;
using CartingService.BusinessLogicLayer;
using CartingService.Configuration;
using CartingService.DataAccessLayer;
using CartingService.MappingProfiles;
using CartingService.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Messaging.Abstractions;
using Messaging.Consumer;
using RabbitMQ.Client;
 
 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options =>
    {
        options.RespectBrowserAcceptHeader = true;
        options.ReturnHttpNotAcceptable = true;

        options.Filters.Add(new ProducesAttribute("application/json"));
        options.Filters.Add(new ConsumesAttribute("application/json"));
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(ImageProfile).Assembly);
builder.Services.AddHostedService<ItemsMessagesConsumerService>();
builder.Services.AddLogging();
var cosmosOptions = new CosmosDbSettings();
builder.Services.AddTransient(provider =>
{
    var cosmosClientOptions = new CosmosClientOptions()
    {
        SerializerOptions = new CosmosSerializationOptions()
        {
            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
        }
    };

    if (!builder.Environment.IsDevelopment())
        return new CosmosClient(cosmosOptions.CosmosDbConnectionString, cosmosClientOptions);

    cosmosClientOptions.HttpClientFactory = () =>
    {
        HttpMessageHandler httpMessageHandler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        return new HttpClient(httpMessageHandler);
    };
    cosmosClientOptions.ConnectionMode = ConnectionMode.Gateway;
    return new CosmosClient(cosmosOptions.CosmosDbConnectionString, cosmosClientOptions);
});

builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigureOptions>();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(xmlFilePath);
});

builder.Services.AddTransient<CosmosDbDataService>();
builder.Configuration.Bind("CosmosDbSettings", cosmosOptions);
builder.Services.AddTransient<ICartRepository>(provider => new CosmosDbCartRepository(provider.GetRequiredService<CosmosDbDataService>(),
    cosmosOptions.CosmosDbName,
    cosmosOptions.CosmosDbContainerName,
    cosmosOptions.CosmosDbContainerPartitionKey,
    cosmosOptions.CosmosDbThroughput));
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddValidatorsFromAssembly(typeof(ItemsValidator).Assembly);

builder.Services.AddSingleton<IQueueConsumer, RabbitQueueConsumer>();
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
    app.UseSwagger(options =>
    {
        options.PreSerializeFilters.Add((swagger, req) =>
        {
            swagger.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = $"https://{req.Host}" } };
        });
    });

    IApiVersionDescriptionProvider apiVersionDescriptionProvider =
        app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    
    app.UseSwaggerUI(options =>
    {
        foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"../swagger/{desc.GroupName}/swagger.json", desc.ApiVersion.ToString());
            options.DefaultModelsExpandDepth(-1);
            options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
