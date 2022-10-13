using API.Filters;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Application.Mappings;
using CatalogService.Application.UseCases.Catalog.Queries;
using CatalogService.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(
    options => options.Filters.Add<ApiExceptionFilterAttribute>());
builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => optionsBuilder.UseInMemoryDatabase("catalog"));
builder.Services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(GetCatalogsQuery).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryMapping).Assembly);

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
