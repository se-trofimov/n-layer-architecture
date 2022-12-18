using CatalogGraphQL.Persistence;
using CatalogGraphQL.Schema;
using CatalogGraphQL.Services;
using GraphQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => 
    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString")));


builder.Services.AddScoped<CategoriesSchema>();

builder.Services.AddGraphQL(qlBuilder => qlBuilder.AddSystemTextJson()
    .AddSchema<CategoriesSchema>()
    .AddGraphTypes(typeof(CategoriesQuery).Assembly)
    .AddSystemTextJson());



var app = builder.Build();

 
if (app.Environment.IsDevelopment())
{

}

app.UseDefaultFiles();
app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseStaticFiles();

app.UseGraphQLGraphiQL();
app.UseGraphQLAltair();
app.UseGraphQL("/graphql");
app.UseGraphQLPlayground(
    "/",                               // url to host Playground at
    new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/graphql",         // url of GraphQL endpoint
        SubscriptionsEndPoint = "/graphql",   // url of GraphQL endpoint
    });
app.Run();
