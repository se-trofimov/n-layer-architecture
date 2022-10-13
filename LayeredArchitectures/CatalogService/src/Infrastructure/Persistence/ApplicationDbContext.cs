using System.Reflection;
using CatalogService.Application.Common.Interfaces;
using CatalogService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Infrastructure.Persistence;

public class ApplicationDbContext :DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(new Category()
        {
            Id = 1, Name = "Electronics", Image = "electronics.png"
        });
        modelBuilder.Entity<Category>().HasData(new Category()
        {
            Id = 2, Name = "Laptops",
        });
        modelBuilder.Entity<Category>().HasData(new Category()
        {
            Id = 3,
            Name = "Household equipment",
            Image = "equipment.png"
        });
        modelBuilder.Entity<Item>().HasData(new Item[]
        {
            new Item()
            {
                Id = 1,
                Amount = 10,
                Price = 1200,
                Name = "HP EliteBook 820",
                Description = "Some laptop",
                Image = "hp-elitebook.png",
                CategoryId = 2
            },
            new Item()
            {
                Id = 2,
                Amount = 10,
                Price = 1100,
                Name = "MSI Katana",
                Description = "Some laptop",
                Image = "katana.png",
                CategoryId = 2
            }
        });
    }
}
