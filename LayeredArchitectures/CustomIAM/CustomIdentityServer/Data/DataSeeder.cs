using CustomIdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentityServer.Data;

public class DataSeeder
{
    public static async Task SeedDataAsync(IdentityDbContext context)
    {
        var permissions = await context.Permissions.ToListAsync();
        if (!permissions.Exists(x => x.Title == "Catalog.Read"))
            context.Permissions.Add(new Permission() { Title = "Catalog.Read", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Catalog.Update"))
            context.Permissions.Add(new Permission() { Title = "Catalog.Update", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Catalog.Create"))
            context.Permissions.Add(new Permission() { Title = "Catalog.Create", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Catalog.Delete"))
            context.Permissions.Add(new Permission() { Title = "Catalog.Delete", Id = Guid.NewGuid() });

        var roles = await context.Roles.ToListAsync();
        if (!roles.Exists(x => x.Title == "Manager"))
            context.Roles.Add(new Role() { Title = "Manager", Id = Guid.NewGuid()});
        if (!roles.Exists(x => x.Title == "Buyer"))
            context.Roles.Add(new Role() { Title = "Buyer", Id = Guid.NewGuid() });

        await context.SaveChangesAsync();

        var manager = context.Roles
            .Include(x=>x.Permissions)
            .First(x => x.Title == "Manager");

        if (manager.Permissions.FirstOrDefault(x => x.Title == "Catalog.Read") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Catalog.Read");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Catalog.Update") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Catalog.Update");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Catalog.Delete") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Catalog.Delete");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Catalog.Create") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Catalog.Create");
            manager.Permissions.Add(permission);
        }

        var buyer = context.Roles
            .Include(x => x.Permissions)
            .First(x => x.Title == "Buyer");

        if (buyer.Permissions.FirstOrDefault(x => x.Title == "Catalog.Read") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Catalog.Read");
            buyer.Permissions.Add(permission);
        }

        await context.SaveChangesAsync();

        
        if (context.Users.FirstOrDefault(x=>x.Name == "Super Administrator") is null)
        {
            PasswordEncrypt passwordEncrypt = new PasswordEncrypt();
            
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "super.admin@gmail.com",
                Name = "Super Administrator",
                Password = passwordEncrypt.GeneratePasswordHashUsingSalt("password")
            };

            user.Roles.Add(manager);

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }
}