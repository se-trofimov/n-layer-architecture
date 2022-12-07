using CustomIdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentityServer.Data;

public class DataSeeder
{
    public static async Task SeedDataAsync(IdentityDbContext context)
    {
        var permissions = await context.Permissions.ToListAsync();
        if (!permissions.Exists(x => x.Title == "Category.Read"))
            context.Permissions.Add(new Permission() { Title = "Category.Read", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Category.Update"))
            context.Permissions.Add(new Permission() { Title = "Category.Update", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Category.Create"))
            context.Permissions.Add(new Permission() { Title = "Category.Create", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Category.Delete"))
            context.Permissions.Add(new Permission() { Title = "Category.Delete", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Items.Read"))
            context.Permissions.Add(new Permission() { Title = "Items.Read", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Items.Update"))
            context.Permissions.Add(new Permission() { Title = "Items.Update", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Items.Create"))
            context.Permissions.Add(new Permission() { Title = "Items.Create", Id = Guid.NewGuid() });
        if (!permissions.Exists(x => x.Title == "Items.Delete"))
            context.Permissions.Add(new Permission() { Title = "Items.Delete", Id = Guid.NewGuid() });

        var roles = await context.Roles.ToListAsync();
        if (!roles.Exists(x => x.Title == "Administrator"))
            context.Roles.Add(new Role() { Title = "Administrator", Id = Guid.NewGuid() });
        if (!roles.Exists(x => x.Title == "Manager"))
            context.Roles.Add(new Role() { Title = "Manager", Id = Guid.NewGuid()});
        if (!roles.Exists(x => x.Title == "Buyer"))
            context.Roles.Add(new Role() { Title = "Buyer", Id = Guid.NewGuid() });

        await context.SaveChangesAsync();

        var manager = context.Roles
            .Include(x=>x.Permissions)
            .First(x => x.Title == "Manager");

        if (manager.Permissions.FirstOrDefault(x => x.Title == "Category.Read") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Read");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Category.Update") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Update");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Category.Delete") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Delete");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Category.Create") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Create");
            manager.Permissions.Add(permission);
        }

        if (manager.Permissions.FirstOrDefault(x => x.Title == "Items.Read") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Read");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Items.Update") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Update");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Items.Delete") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Delete");
            manager.Permissions.Add(permission);
        }
        if (manager.Permissions.FirstOrDefault(x => x.Title == "Items.Create") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Create");
            manager.Permissions.Add(permission);
        }

        var admin = context.Roles
            .Include(x => x.Permissions)
            .First(x => x.Title == "Administrator");

        if (manager.Permissions.FirstOrDefault(x => x.Title == "Category.Read") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Read");
            admin.Permissions.Add(permission);
        }
        if (admin.Permissions.FirstOrDefault(x => x.Title == "Category.Update") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Update");
            admin.Permissions.Add(permission);
        }
        if (admin.Permissions.FirstOrDefault(x => x.Title == "Category.Delete") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Delete");
            admin.Permissions.Add(permission);
        }
        if (admin.Permissions.FirstOrDefault(x => x.Title == "Category.Create") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Create");
            admin.Permissions.Add(permission);
        }

        if (admin.Permissions.FirstOrDefault(x => x.Title == "Items.Read") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Read");
            admin.Permissions.Add(permission);
        }
        if (admin.Permissions.FirstOrDefault(x => x.Title == "Items.Update") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Update");
            admin.Permissions.Add(permission);
        }
        if (admin.Permissions.FirstOrDefault(x => x.Title == "Items.Delete") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Delete");
            admin.Permissions.Add(permission);
        }
        if (admin.Permissions.FirstOrDefault(x => x.Title == "Items.Create") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Create");
            admin.Permissions.Add(permission);
        }

        var buyer = context.Roles
            .Include(x => x.Permissions)
            .First(x => x.Title == "Buyer");

        if (buyer.Permissions.FirstOrDefault(x => x.Title == "Category.Read") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Category.Read");
            buyer.Permissions.Add(permission);
        }
        if (buyer.Permissions.FirstOrDefault(x => x.Title == "Items.Read") is null)
        {
            var permission = context.Permissions.First(x => x.Title == "Items.Read");
            buyer.Permissions.Add(permission);
        }

        await context.SaveChangesAsync();

        
        if (context.Users.FirstOrDefault(x=>x.Name == "Administrator") is null)
        {
            PasswordEncrypt passwordEncrypt = new PasswordEncrypt();
            
            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "super.admin@gmail.com",
                Name = "Super Administrator",
                Password = passwordEncrypt.GeneratePasswordHashUsingSalt("password")
            };

            user.Roles.Add(admin);

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        if (context.Users.FirstOrDefault(x => x.Name == "Manager") is null)
        {
            PasswordEncrypt passwordEncrypt = new PasswordEncrypt();

            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "super.manager@gmail.com",
                Name = "Super Manager",
                Password = passwordEncrypt.GeneratePasswordHashUsingSalt("password")
            };

            user.Roles.Add(manager);

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        if (context.Users.FirstOrDefault(x => x.Name == "Buyer") is null)
        {
            PasswordEncrypt passwordEncrypt = new PasswordEncrypt();

            User user = new User()
            {
                Id = Guid.NewGuid(),
                Email = "super.Buyer@gmail.com",
                Name = "Super Buyer",
                Password = passwordEncrypt.GeneratePasswordHashUsingSalt("password")
            };

            user.Roles.Add(buyer);

            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
    }
}