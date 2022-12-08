using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Configuration
    .AddOcelot(builder.Environment);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:ClientSecret"])),
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
        });

builder.Services.AddOcelot()
    .AddCacheManager(x =>
    {
        x.WithDictionaryHandle();
    })
    .AddSingletonDefinedAggregator<ItemsPropertiesAggregator>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseMiddleware<CodeHandlerMiddleware>();
app.UseAuthorization();


app.UseOcelot(configuration =>
{
    configuration.AuthorizationMiddleware = AuthMiddleware.AuthorizationMiddleware;
});
app.Run();
