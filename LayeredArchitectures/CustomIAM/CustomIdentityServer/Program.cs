using CustomIdentityServer;
using CustomIdentityServer.Data;
using CustomIdentityServer.UIModelsValidation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssembly(typeof(CreationUserValidator).Assembly);
builder.Services.AddScoped<UserIdentityService>();
builder.Services.AddSingleton<PasswordEncrypt>();
builder.Services.AddDbContext<IdentityDbContext>(optionsBuilder => optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString")));
builder.Services.AddSingleton<IJwtUtils>(new JwtUtils(builder.Configuration["Issuer"], builder.Configuration["Audience"]));
builder.Services.AddMemoryCache();

var app = builder.Build();

var scope = app.Services.CreateScope();
await DataSeeder.SeedDataAsync(scope.ServiceProvider.GetRequiredService<IdentityDbContext>());

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