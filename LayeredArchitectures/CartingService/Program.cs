using CartingService.BusinessLogicLayer;
using CartingService.Configuration;
using CartingService.DataAccessLayer;
using CartingService.MappingProfiles;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ImageProfile).Assembly);

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
builder.Services.AddTransient<CosmosDbDataService>();
builder.Configuration.Bind("CosmosDbSettings", cosmosOptions);
builder.Services.AddTransient<ICartRepository>(provider => new CosmosDbCartRepository(provider.GetRequiredService<CosmosDbDataService>(),
    cosmosOptions.CosmosDbName,
    cosmosOptions.CosmosDbContainerName,
    cosmosOptions.CosmosDbContainerPartitionKey,
    cosmosOptions.CosmosDbThroughput));
builder.Services.AddTransient<ICartService, CartService>();

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
