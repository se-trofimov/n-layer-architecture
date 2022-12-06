using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddOcelot(builder.Environment);
builder.Services.AddAuthentication();
builder.Services.AddOcelot()
    .AddSingletonDefinedAggregator<ItemsPropertiesAggregator>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//app.UseAuthorization();


app.UseOcelot();
app.Run();
