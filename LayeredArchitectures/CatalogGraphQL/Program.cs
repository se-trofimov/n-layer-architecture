using CatalogGraphQL.Persistence;
using CatalogGraphQL.Schema;
using CatalogGraphQL.Services;
using GraphQL;
using GraphQL.Server.Ui.GraphiQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IItemsService, ItemsService>();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DbConnectionString")));


builder.Services.AddScoped<CategoriesSchema>();

builder.Services.AddGraphQL(qlBuilder =>
    qlBuilder.AddSchema<CategoriesSchema>()
    .AddSchema<ItemsSchema>()
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



app.UseGraphQL<CategoriesSchema>("/api/categories");
app.UseGraphQL<ItemsSchema>("/api/items");

//app.UseGraphQLPlayground("/ui/categories", new PlaygroundOptions { GraphQLEndPoint = "/api/categories" });
//app.UseGraphQLPlayground("/ui/items", new PlaygroundOptions { GraphQLEndPoint = "/api/items" });


app.UseGraphQLGraphiQL(path: "/ui/categories", new GraphiQLOptions() { GraphQLEndPoint = "/api/categories" });
app.UseGraphQLGraphiQL(path: "/ui/items", new GraphiQLOptions() { GraphQLEndPoint = "/api/items" });

app.Run();
