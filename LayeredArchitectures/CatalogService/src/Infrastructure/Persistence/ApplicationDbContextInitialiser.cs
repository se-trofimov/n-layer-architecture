using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CatalogService.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!_context.Categories.Any())
        {
            _context.Categories.Add(new Category()
            {
                Id = 1,
                Name = "Electronics",
                Image = "electronics.png"
            });
            _context.Categories.Add(new Category()
            {
                Id = 2,
                Name = "Laptops",
                ParentCategoryId = 1,
                Items = new List<Item>()
                {
                    new Item()
                    {
                        Id = 1,
                        Amount = 10,
                        Price = 1200,
                        Name = "HP EliteBook 820",
                        Description = "Some laptop",
                        Image = "hp-elitebook.png"
                    },
                    new Item()
                    {
                        Id = 2,
                        Amount = 10,
                        Price = 1100,
                        Name = "MSI Katana",
                        Description = "Some laptop",
                        Image = "katana.png"
                    }
                }
            });
            _context.Categories.Add(new Category()
            {
                Id = 3,
                Name = "Household equipment",
            });
            await _context.SaveChangesAsync();
        }
    }
}
