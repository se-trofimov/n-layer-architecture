using CustomIdentityServer.Data;
using CustomIdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentityServer;

public class UserIdentityService
{
    private readonly IdentityDbContext _identityDbContext;
    private readonly PasswordEncrypt _passwordEncrypt;

    public UserIdentityService(IdentityDbContext identityDbContext, PasswordEncrypt passwordEncrypt)
    {
        _identityDbContext = identityDbContext ?? throw new ArgumentNullException(nameof(identityDbContext));
        _passwordEncrypt = passwordEncrypt ?? throw new ArgumentNullException(nameof(passwordEncrypt));
    }

    public async Task<User> CreateUserAsync(string userName, string email, string password)
    {
        var userExists = await _identityDbContext.Users
            .AnyAsync(x => x.Email == email.ToLower());

        if (userExists)
            throw new InvalidOperationException($"User with email {email} already exists");

        User user = new()
        {
            Name = userName,
            Email = email,
            Password = _passwordEncrypt.GeneratePasswordHashUsingSalt(password)
        };

        _identityDbContext.Users.Add(user);
        await _identityDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _identityDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}
